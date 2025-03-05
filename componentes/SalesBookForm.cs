using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using SalesBookApp.Repositories;
using SalesBookApp.Services;
using apos_gestor_caja.Domain.Models;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace SalesBookApp
{
    public partial class SalesBookForm : Form
    {
        private readonly SalesRepository _salesRepository;
        private readonly ExcelService _excelService;
        private List<SalesRecord> _currentRecords;
        private List<SalesRecord> _filteredRecords; // Para mantener una copia filtrada
        private int _maxInvoiceLength;
        private bool _isDetailed;

        public SalesBookForm()
        {
            InitializeComponent();
            _salesRepository = new SalesRepository();
            _excelService = new ExcelService();

            datePickerStart.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            datePickerEnd.Value = DateTime.Now;

            ConfigureDataGridViewBasic();
            btnExport.Visible = false;
            // Agregar eventos para el manejo de las cajas
            txtCashierStart.LostFocus += TxtCashier_LostFocus;
            txtCashierEnd.LostFocus += TxtCashier_LostFocus;
        }

        private void TxtCashier_LostFocus(object sender, EventArgs e)
        {
            TextBox currentTextBox = (TextBox)sender;
            TextBox otherTextBox = currentTextBox == txtCashierStart ? txtCashierEnd : txtCashierStart;
            if (!string.IsNullOrWhiteSpace(currentTextBox.Text))
            {
                // Formatear el número actual
                if (int.TryParse(currentTextBox.Text, out int number))
                {
                    currentTextBox.Text = number.ToString("00");
                    // Si la otra caja está vacía, copiar el valor formateado
                    if (string.IsNullOrWhiteSpace(otherTextBox.Text))
                    {
                        otherTextBox.Text = currentTextBox.Text;
                    }
                }
            }
        }

        private void ConfigureDataGridViewBasic()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.MultiSelect = false;

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Date",
                DataPropertyName = "Date",
                HeaderText = "Fecha",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy", Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Rif",
                DataPropertyName = "Rif",
                HeaderText = "Nro de RIF",
                ReadOnly = true
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CustomerName",
                DataPropertyName = "CustomerName",
                HeaderText = "Nombre o Razón Social",
                ReadOnly = true
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PrinterSerial",
                DataPropertyName = "PrinterSerial",
                HeaderText = "Nro Máquina Fiscal",
                ReadOnly = true
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ReportZ",
                DataPropertyName = "ReportZ",
                HeaderText = "N Reporte Z"
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DocumentType",
                DataPropertyName = "DocumentType",
                HeaderText = "Tipo Documento"
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StartInvoice",
                DataPropertyName = "StartInvoice",
                HeaderText = "Factura Inicial"
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "EndInvoice",
                DataPropertyName = "EndInvoice",
                HeaderText = "Factura Final"
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IgtfBase",
                DataPropertyName = "IgtfBase",
                HeaderText = "Base IGTF",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IgtfAmount",
                DataPropertyName = "IgtfAmount",
                HeaderText = "IGTF 3%",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TotalInvoice",
                DataPropertyName = "TotalInvoice",
                HeaderText = "Total Ventas",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ExemptAmount",
                DataPropertyName = "ExemptAmount",
                HeaderText = "Exentas/Exoneradas",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Base8",
                DataPropertyName = "Base8",
                HeaderText = "Base del 8%",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Iva8",
                DataPropertyName = "Iva8",
                HeaderText = "IVA 8%",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TaxableBase",
                DataPropertyName = "TaxableBase",
                HeaderText = "Base del 16%",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Vat16",
                DataPropertyName = "Vat16",
                HeaderText = "IVA 16%",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
            });

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Establecer estilos básicos que no cambian al navegar
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(0, 102, 204);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridView1.ColumnHeadersHeight = 40;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.BackgroundColor = System.Drawing.Color.White;
            dataGridView1.GridColor = System.Drawing.Color.LightGray;

            // Habilitar doble búfer para reducir parpadeo
            SetDoubleBuffered(dataGridView1, true);

            // Agregar evento para clic en encabezado
            dataGridView1.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridView1_ColumnHeaderMouseClick);
        }

        private void ApplyDataGridViewEffects()
        {
            dataGridView1.SuspendLayout();

            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                for (int i = 8; i <= 15; i++)
                {
                    row.Cells[i].Style.ForeColor = System.Drawing.Color.FromArgb(0, 102, 0);
                }

                if (_maxInvoiceLength > 0)
                {
                    if (!string.IsNullOrEmpty(row.Cells["StartInvoice"].Value?.ToString()))
                    {
                        row.Cells["StartInvoice"].Value = row.Cells["StartInvoice"].Value.ToString().PadLeft(_maxInvoiceLength, '0');
                    }
                    if (!string.IsNullOrEmpty(row.Cells["EndInvoice"].Value?.ToString()))
                    {
                        row.Cells["EndInvoice"].Value = row.Cells["EndInvoice"].Value.ToString().PadLeft(_maxInvoiceLength, '0');
                    }
                }

                if (string.IsNullOrEmpty(row.Cells["CustomerName"].Value?.ToString()))
                {
                    row.Cells["CustomerName"].Value = "Resumen de Ventas";
                }
            }

            dataGridView1.ResumeLayout();
            dataGridView1.Refresh();
        }

        private void CalculateMaxInvoiceLength()
        {
            _maxInvoiceLength = 0;
            if (_currentRecords != null)
            {
                foreach (var record in _currentRecords)
                {
                    if (!string.IsNullOrEmpty(record.StartInvoice))
                        _maxInvoiceLength = Math.Max(_maxInvoiceLength, record.StartInvoice.Length);
                    if (!string.IsNullOrEmpty(record.EndInvoice))
                        _maxInvoiceLength = Math.Max(_maxInvoiceLength, record.EndInvoice.Length);
                }
            }
        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            if (datePickerEnd.Value < datePickerStart.Value)
            {
                MessageBox.Show("La fecha final debe ser mayor o igual a la fecha inicial",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int? cashierStart = null;
            int? cashierEnd = null;

            if (!string.IsNullOrWhiteSpace(txtCashierStart.Text))
            {
                if (!int.TryParse(txtCashierStart.Text, out int start))
                {
                    MessageBox.Show("El número de caja inicial no es válido",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                cashierStart = start;
            }

            if (!string.IsNullOrWhiteSpace(txtCashierEnd.Text))
            {
                if (!int.TryParse(txtCashierEnd.Text, out int end))
                {
                    MessageBox.Show("El número de caja final no es válido",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                cashierEnd = end;
            }

            if (cashierStart.HasValue && cashierEnd.HasValue && cashierStart > cashierEnd)
            {
                MessageBox.Show("El número de caja inicial debe ser menor o igual al final",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                btnGenerate.Enabled = false;
                btnExport.Enabled = false;
                Cursor = Cursors.WaitCursor;

                // Reemplaza el MessageBox con el diálogo personalizado
                using (var reportDialog = new ReportTypeDialog())
                {
                    if (reportDialog.ShowDialog() == DialogResult.Cancel)
                        return; // Si el usuario cierra el diálogo sin elegir

                    // Usamos la propiedad IsDetailed del diálogo
                    _isDetailed = reportDialog.IsDetailed;
                }

                // Sólo una llamada al repositorio
                _currentRecords = await _salesRepository.GetSalesRecordsAsync(
                    datePickerStart.Value, datePickerEnd.Value, _isDetailed, cashierStart, cashierEnd);

                if (_currentRecords.Count == 0)
                {
                    MessageBox.Show("No se encontraron registros para el período seleccionado",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                CalculateMaxInvoiceLength();

                dataGridView1.SetDoubleBuffered(false);
                dataGridView1.SuspendLayout();

                _filteredRecords = _currentRecords; // Inicialmente, sin filtro
                dataGridView1.DataSource = _filteredRecords;

                ApplyDataGridViewEffects();
                dataGridView1.ResumeLayout();

                dataGridView1.SetDoubleBuffered(true);

                dataGridView1.Refresh();

                btnExport.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los registros de ventas: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGenerate.Enabled = true;
                btnExport.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentRecords == null || _currentRecords.Count == 0)
                {
                    MessageBox.Show("No hay datos para exportar",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var excelData = _excelService.GenerateSalesBookExcel(
                    _filteredRecords, datePickerStart.Value, datePickerEnd.Value, _isDetailed);

                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Excel Files|*.xlsx";
                    saveDialog.FileName = $"Libro_Ventas_{(_isDetailed ? "Detallado" : "Resumido")}_{datePickerStart.Value:yyyyMMdd}_{datePickerEnd.Value:yyyyMMdd}.xlsx";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(saveDialog.FileName, excelData);
                        MessageBox.Show("Libro de ventas generado exitosamente",
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el libro de ventas: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCashier_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo permitir dígitos y teclas de control
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["PrinterSerial"].Index && _currentRecords != null && _currentRecords.Count > 0)
            {
                // Obtener máquinas fiscales únicas
                var uniqueMachines = _currentRecords.Select(r => r.PrinterSerial)
                                                    .Distinct()
                                                    .Where(m => !string.IsNullOrEmpty(m))
                                                    .OrderBy(m => m)
                                                    .ToList();

                if (uniqueMachines.Count == 0) return;

                // Crear menú contextual
                ContextMenuStrip menu = new ContextMenuStrip();
                menu.Items.Add("Mostrar todas", null, (s, ev) =>
                {
                    _filteredRecords = _currentRecords;
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = _filteredRecords;
                    ApplyDataGridViewEffects();
                });

                foreach (var machine in uniqueMachines)
                {
                    menu.Items.Add(machine, null, (s, ev) =>
                    {
                        _filteredRecords = _currentRecords.Where(r => r.PrinterSerial == machine).ToList();
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = _filteredRecords;
                        ApplyDataGridViewEffects();
                    });
                }

                // Mostrar el menú en la posición del clic
                Point headerPosition = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, -1, true).Location;
                menu.Show(dataGridView1, new Point(headerPosition.X + e.X, headerPosition.Y + e.Y));
            }
        }

        private void SetDoubleBuffered(Control control, bool enabled)
        {
            var doubleBufferPropertyInfo = control.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            doubleBufferPropertyInfo?.SetValue(control, enabled, null);
        }

        private void SalesBookForm_Load(object sender, EventArgs e)
        {

        }
    }

    public static class DataGridViewExtensions
    {
        public static void SetDoubleBuffered(this DataGridView dgv, bool setting)
        {
            typeof(DataGridView).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(dgv, setting, null);
        }
    }
}

public class ReportTypeDialog : Form
{
    private Button btnResumido;
    private Button btnDetallado;
    private Label lblMessage;

    public bool IsDetailed { get; private set; }

    public ReportTypeDialog()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        this.Text = "Tipo de Reporte";
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.CenterParent;
        this.ShowInTaskbar = false;
        this.Size = new Size(350, 180);
        this.Font = new Font("Segoe UI", 9F);

        // Message label
        lblMessage = new Label
        {
            Text = "¿Qué tipo de reporte desea generar?",
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 60
        };
        this.Controls.Add(lblMessage);

        // Panel for buttons
        Panel buttonPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 60
        };
        this.Controls.Add(buttonPanel);

        // Resumido button
        btnResumido = new Button
        {
            Text = "Resumido",
            Size = new Size(100, 35),
            Location = new Point(60, 10),
            DialogResult = DialogResult.Yes
        };
        btnResumido.Click += (s, e) =>
        {
            IsDetailed = false;
            this.DialogResult = DialogResult.Yes;
            this.Close();
        };
        buttonPanel.Controls.Add(btnResumido);

        // Detallado button
        btnDetallado = new Button
        {
            Text = "Detallado",
            Size = new Size(100, 35),
            Location = new Point(180, 10),
            DialogResult = DialogResult.No
        };
        btnDetallado.Click += (s, e) =>
        {
            IsDetailed = true;
            this.DialogResult = DialogResult.No;
            this.Close();
        };
        buttonPanel.Controls.Add(btnDetallado);
    }
}