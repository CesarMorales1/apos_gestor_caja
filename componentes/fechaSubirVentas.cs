using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using apos_gestor_caja.service;

namespace apos_gestor_caja.Forms
{
    public partial class VentasForm : Form
    {
        private readonly UploadVentas _archivoService;

        public VentasForm()
        {
            InitializeComponent();
            _archivoService = new UploadVentas();
            dtpFechaInicio.Value = DateTime.Today.AddDays(-7);
            dtpFechaFin.Value = DateTime.Today;
            txtCaja.Text = "";
        }

        private async void btnObtenerArchivos_Click(object sender, EventArgs e)
        {
            try
            {
                lblCargando.Text = "Buscando...";
                lblCargando.Visible = true;
                btnObtenerArchivos.Enabled = false;
                btnSubirVentas.Enabled = false;
                Application.DoEvents();

                string caja = txtCaja.Text.Trim();
                string cajaParam = string.IsNullOrWhiteSpace(caja) ? null : caja;

                var archivos = await _archivoService.ObtenerArchivosVentasAsync(dtpFechaInicio.Value, dtpFechaFin.Value, cajaParam);
                lstArchivos.Items.Clear();
                foreach (var archivo in archivos)
                {
                    lstArchivos.Items.Add(archivo);
                }

                if (archivos.Count == 0)
                {
                    MessageBox.Show("No se encontraron archivos de ventas en el rango de fechas y caja especificados.",
                                    "Sin Resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los archivos: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                lblCargando.Visible = false;
                btnObtenerArchivos.Enabled = true;
                btnSubirVentas.Enabled = true;
            }
        }

        private async void btnSubirVentas_Click(object sender, EventArgs e)
        {
            try
            {
                lblCargando.Text = "Subiendo ventas...";
                lblCargando.Visible = true;
                btnObtenerArchivos.Enabled = false;
                btnSubirVentas.Enabled = false;
                Application.DoEvents();

                string caja = txtCaja.Text.Trim();
                string cajaParam = string.IsNullOrWhiteSpace(caja) ? null : caja;

                bool resultado = await _archivoService.SubirArchivosVentasAsync(dtpFechaInicio.Value, dtpFechaFin.Value, cajaParam);
                if (resultado)
                {
                    MessageBox.Show("Ventas subidas con éxito.",
                                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se encontraron archivos para subir en el rango especificado.",
                                    "Sin Resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al subir las ventas: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                lblCargando.Visible = false;
                btnObtenerArchivos.Enabled = true;
                btnSubirVentas.Enabled = true;
            }
        }

        private void txtCaja_TextChanged(object sender, EventArgs e)
        {

        }

        private void VentasForm_Load(object sender, EventArgs e)
        {

        }
    }
}