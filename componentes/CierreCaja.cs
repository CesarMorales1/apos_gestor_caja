using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using apos_gestor_caja.service;

namespace apos_gestor_caja.componentes
{
    public partial class CierreCaja : Form
    {
        private readonly CuadreCajaService _cuadreCajaService;
        private List<CuadreCajaService.informacionCuadreCaja> _ultimasLineasAMostrar;

        public CierreCaja()
        {
            InitializeComponent();
            _cuadreCajaService = new CuadreCajaService();
            ConfigurarColumnasDataGridView();
        }

        private void ConfigurarColumnasDataGridView()
        {
            dataGridViewCierre.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCierre.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dataGridViewCierre.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dataGridViewCierre.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridViewCierre.Columns.Add("Abreviacion", "Abrev.");
            dataGridViewCierre.Columns.Add("TipoPago", "Tipo Pago");
            dataGridViewCierre.Columns.Add("CantidadSistema", "Cant. Sistema");
            dataGridViewCierre.Columns.Add("ConfirmacionSistema", "Conf. Sistema");
            dataGridViewCierre.Columns.Add("MontoRecibido", "Monto Recibido");
            dataGridViewCierre.Columns.Add("BaucheRecibido", "Bauche Recibido");

            // Configurar editabilidad por columna
            dataGridViewCierre.Columns["Abreviacion"].ReadOnly = true;
            dataGridViewCierre.Columns["TipoPago"].ReadOnly = true;
            dataGridViewCierre.Columns["CantidadSistema"].ReadOnly = true;
            dataGridViewCierre.Columns["ConfirmacionSistema"].ReadOnly = true;
            dataGridViewCierre.Columns["MontoRecibido"].ReadOnly = false;
            dataGridViewCierre.Columns["BaucheRecibido"].ReadOnly = false;

            // Evento para manejar el cambio de fila
            dataGridViewCierre.SelectionChanged += DataGridViewCierre_SelectionChanged;
        }

        private void TextBoxNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private async void ButtonBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                buttonBuscar.Enabled = false;
                buttonBuscar.Text = "Buscando...";

                DateTime fecha = dateTimePickerFecha.Value;
                if (!int.TryParse(textBoxCajeroId.Text, out int cajeroId) || cajeroId < 1 || string.IsNullOrWhiteSpace(textBoxCajeroId.Text))
                {
                    MessageBox.Show("Por favor, ingrese un ID de cajero válido (número positivo).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!int.TryParse(textBoxCaja.Text, out int cajaId) || cajaId < 0 || string.IsNullOrWhiteSpace(textBoxCaja.Text))
                {
                    MessageBox.Show("Por favor, ingrese un número de caja válido (0 o mayor).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var cierres = await _cuadreCajaService.ObtenerCierreCaja(cajeroId, fecha, cajaId);
                Console.WriteLine($"Cierres obtenidos: {cierres.Count}");

                if (cierres == null || !cierres.Any())
                {
                    MessageBox.Show("No se encontraron datos para el cierre de caja.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var lineasAMostrar = await _cuadreCajaService.LineasAMostrar(cierres, cajeroId);
                Console.WriteLine($"LineasAMostrar procesadas: {lineasAMostrar.Count}");

                if (lineasAMostrar == null || !lineasAMostrar.Any())
                {
                    MessageBox.Show("No se procesaron líneas para mostrar. Verifique el cajero o los datos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                dataGridViewCierre.Rows.Clear();

                // Tipos que se resumen
                var tiposResumidos = new[] { "EF", "PP", "USD" };
                var tiposTarjetas = new[] { "TD", "TB" };
                var lineasResumidas = new List<CuadreCajaService.informacionCuadreCaja>();

                // Resumir Efectivo, PayPal y Dólares
                foreach (var tipo in tiposResumidos)
                {
                    var lineasDelTipo = lineasAMostrar.Where(l => l.Abreviacion.Equals(tipo, StringComparison.OrdinalIgnoreCase)).ToList();
                    if (lineasDelTipo.Any())
                    {
                        var resumen = new CuadreCajaService.informacionCuadreCaja
                        {
                            Abreviacion = tipo,
                            TipoPago = lineasDelTipo.First().TipoPago,
                            BaucheRecibido = 1.00, // No editable
                            MontoRecibido = 0 // Para EF, PP y USD
                        };

                        if (tipo.Equals("USD", StringComparison.OrdinalIgnoreCase))
                        {
                            // Para USD: CantidadSistema = tasa de cambio, ConfirmacionSistema = suma de cantidad en dólares
                            var cierreUSD = cierres.FirstOrDefault(c => c.Emisor == 75);
                            resumen.CantidadSistema = cierreUSD?.Monto ?? 0; // Tasa de cambio
                            resumen.ConfirmacionSistema = lineasDelTipo.Sum(l => l.CantidadSistema); // Suma de cantidad en dólares
                        }
                        else
                        {
                            // Para EF y PP: CantidadSistema = suma, ConfirmacionSistema = 1.00
                            resumen.CantidadSistema = lineasDelTipo.Sum(l => l.CantidadSistema);
                            resumen.ConfirmacionSistema = 1.00;
                        }

                        lineasResumidas.Add(resumen);
                    }
                }

                // Otros métodos (tarjetas, bancos, etc.)
                var lineasNoResumidas = lineasAMostrar
                    .Where(l => !tiposResumidos.Contains(l.Abreviacion, StringComparer.OrdinalIgnoreCase))
                    .OrderBy(l => !l.Abreviacion.StartsWith("td", StringComparison.OrdinalIgnoreCase))
                    .ThenBy(l => l.Abreviacion);

                foreach (var linea in lineasNoResumidas)
                {
                    linea.MontoRecibido = linea.CantidadSistema; // Para los demás
                    linea.BaucheRecibido = 0; // Editable
                    linea.ConfirmacionSistema = 1.00; // Para todos menos USD
                    lineasResumidas.Add(linea);
                }

                // Mostrar en el DataGridView
                foreach (var linea in lineasResumidas)
                {
                    int rowIndex = dataGridViewCierre.Rows.Add(
                        linea.Abreviacion,
                        linea.TipoPago,
                        linea.CantidadSistema.ToString("C2"),
                        linea.ConfirmacionSistema.ToString("C2"),
                        linea.MontoRecibido.ToString("C2"),
                        linea.BaucheRecibido.ToString("C2")
                    );

                    // Configurar editabilidad según el tipo de pago
                    var row = dataGridViewCierre.Rows[rowIndex];
                    if (tiposResumidos.Contains(linea.Abreviacion, StringComparer.OrdinalIgnoreCase) &&
                        !linea.Abreviacion.Equals("USD", StringComparison.OrdinalIgnoreCase))
                    {
                        row.Cells["MontoRecibido"].ReadOnly = false;
                        row.Cells["BaucheRecibido"].ReadOnly = true;
                    }
                    else
                    {
                        row.Cells["MontoRecibido"].ReadOnly = true;
                        row.Cells["BaucheRecibido"].ReadOnly = false;
                    }
                }

                _ultimasLineasAMostrar = lineasResumidas;

                double totalCantidadSistema = lineasResumidas.Sum(l => l.CantidadSistema);
                double totalMontoRecibido = lineasResumidas.Sum(l => l.MontoRecibido);
                double totalBaucheRecibido = lineasResumidas.Sum(l => l.BaucheRecibido);

                labelTotalCantidad.Text = $"Total Cantidad: {totalCantidadSistema:C2}";
                labelTotalMonto.Text = $"Total Monto: {totalMontoRecibido:C2}";
                labelTotalBauche.Text = $"Total Bauche: {totalBaucheRecibido:C2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el cierre de caja: {ex.Message}\n\nDetalles: {ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                buttonBuscar.Enabled = true;
                buttonBuscar.Text = "Buscar";
            }
        }

        private void ButtonLimpiar_Click(object sender, EventArgs e)
        {
            dateTimePickerFecha.Value = DateTime.Now;
            textBoxCajeroId.Text = "4";
            textBoxCaja.Text = "7";
            dataGridViewCierre.Rows.Clear();
            labelTotalCantidad.Text = "Total Cantidad: $0.00";
            labelTotalMonto.Text = "Total Monto: $0.00";
            labelTotalBauche.Text = "Total Bauche: $0.00";
            _ultimasLineasAMostrar = null;
        }

        private void DataGridViewCierre_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Sin tintes, se deja el formato por defecto
        }

        private void DataGridViewCierre_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var row = dataGridViewCierre.Rows[e.RowIndex];
            string abreviacion = row.Cells["Abreviacion"].Value?.ToString();

            if (e.ColumnIndex == dataGridViewCierre.Columns["MontoRecibido"].Index ||
                e.ColumnIndex == dataGridViewCierre.Columns["BaucheRecibido"].Index)
            {
                string editedValue = row.Cells[e.ColumnIndex].Value?.ToString().Replace("$", "").Replace(",", "") ?? "0";
                if (!double.TryParse(editedValue, out double newValue) || newValue < 0)
                {
                    MessageBox.Show("Por favor, ingrese un valor numérico válido y no negativo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    row.Cells[e.ColumnIndex].Value = "0.00";
                    return;
                }

                int index = e.RowIndex;
                if (index >= 0 && index < _ultimasLineasAMostrar.Count)
                {
                    if (e.ColumnIndex == dataGridViewCierre.Columns["MontoRecibido"].Index)
                        _ultimasLineasAMostrar[index].MontoRecibido = newValue;
                    else if (e.ColumnIndex == dataGridViewCierre.Columns["BaucheRecibido"].Index)
                        _ultimasLineasAMostrar[index].BaucheRecibido = newValue;

                    // Recalcular totales
                    double totalMontoRecibido = _ultimasLineasAMostrar.Sum(l => l.MontoRecibido);
                    double totalBaucheRecibido = _ultimasLineasAMostrar.Sum(l => l.BaucheRecibido);
                    labelTotalMonto.Text = $"Total Monto: {totalMontoRecibido:C2}";
                    labelTotalBauche.Text = $"Total Bauche: {totalBaucheRecibido:C2}";
                }
            }
        }

        private void DataGridViewCierre_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewCierre.SelectedRows.Count > 0)
            {
                var row = dataGridViewCierre.SelectedRows[0];
                string abreviacion = row.Cells["Abreviacion"].Value?.ToString();
                // Para USD, PP y EF, el campo editable es MontoRecibido
                var tiposEditableMonto = new[] { "EF", "PP", "USD" };
                // Hacer foco en la celda editable según el tipo
                if (tiposEditableMonto.Contains(abreviacion, StringComparer.OrdinalIgnoreCase))
                {
                    dataGridViewCierre.CurrentCell = row.Cells["MontoRecibido"];
                }
                else // Para bancos y otros métodos, el campo editable es BaucheRecibido
                {
                    dataGridViewCierre.CurrentCell = row.Cells["BaucheRecibido"];
                }
                dataGridViewCierre.BeginEdit(true);
            }
        }
    }
    }