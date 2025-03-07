using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Linq;
using apos_gestor_caja.applicationLayer.interfaces;
using apos_gestor_caja.Domain.Models;
using apos_gestor_caja.Infrastructure.Repositories;
using System.Drawing;
using apos_gestor_caja.ApplicationLayer.Services;

namespace apos_gestor_caja.Forms
{
    public partial class EmisorForm : Form
    {
        private readonly IEmisorService _emisorService;
        private Emisor _emisorSeleccionado;
        private bool _esModoEdicion = false;

        public EmisorForm(IEmisorService emisorService = null)
        {
            InitializeComponent();
            _emisorService = emisorService ?? new EmisorService(new EmisorRepository());
            ConfigurarFormulario();
            ConfigurarEventos();
            CargarEmisoresAsync();

            // Configure responsive behavior
            this.Load += EmisorForm_Load;
            this.Resize += EmisorForm_Resize;
            this.MinimumSize = new Size(800, 600); // Establecer tamaño mínimo para evitar problemas de layout
        }

        private void EmisorForm_Load(object sender, EventArgs e)
        {
            AjustarTamañoSplitContainer();

            // Establecer anclaje de controles para mejor responsive
            emisorListadoGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            emisorSearchTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // Anclajes para el panel de edición
            emisorPanelEdicion.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            emisorLabelTitulo.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            emisorInputNombre.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        }

        private void EmisorForm_Resize(object sender, EventArgs e)
        {
            AjustarTamañoSplitContainer();
        }

        // Manejador para cambios en el DPI o escala de pantalla
        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            AjustarTamañoSplitContainer();
        }

        private void AjustarTamañoSplitContainer()
        {
            // Calculate proportional split based on form width
            int totalWidth = this.ClientSize.Width;
            int totalHeight = this.ClientSize.Height;

            // Ajustar el splitContainer a la ventana completa
            splitContainer.Width = totalWidth;
            splitContainer.Height = totalHeight;

            // Establecer la división apropiada (67% para la lista, 33% para el formulario)
            splitContainer.SplitterDistance = (int)(totalWidth * 0.67);

            // Ajustar los anchos de los controles en el Panel1 (lista)
            emisorSearchTextBox.Width = splitContainer.Panel1.Width - 28; // 14px padding en cada lado
            emisorListadoGrid.Width = splitContainer.Panel1.Width - 28;
            emisorListadoGrid.Height = splitContainer.Panel1.Height - emisorListadoGrid.Top - 14;

            // Ajustar el tamaño del panel de edición
            emisorPanelEdicion.Width = splitContainer.Panel2.Width - 18; // 9px padding en cada lado
            emisorPanelEdicion.Height = splitContainer.Panel2.Height - 32;

            // Ajustar los controles dentro del panel de edición
            emisorInputNombre.Width = emisorPanelEdicion.Width - 34; // 17px padding en cada lado

            // Ajustar el tamaño del TableLayoutPanel de botones
            tableLayoutPanel1.Width = emisorPanelEdicion.Width - 34;

            // Asegurarse de que los botones tienen un tamaño adecuado
            int buttonWidth = (tableLayoutPanel1.Width - 40) / 3; // Espacio para 3 botones con margen
            emisorBotonGuardar.Width = buttonWidth;
            botonEstado.Width = buttonWidth;
            emisorBotonCancelar.Width = buttonWidth;
        }

        private void ConfigurarFormulario()
        {
            emisorListadoGrid.AutoGenerateColumns = false;
            emisorListadoGrid.Columns.Add("Id", "ID");
            emisorListadoGrid.Columns.Add("Nombre", "Nombre");

            // Agregar columna de checkbox para Activo
            DataGridViewCheckBoxColumn activoColumn = new DataGridViewCheckBoxColumn
            {
                Name = "Activo",
                DataPropertyName = "Activo",
                HeaderText = "Activo",
                Width = 70
            };
            emisorListadoGrid.Columns.Add(activoColumn);

            emisorListadoGrid.Columns["Id"].DataPropertyName = "Id";
            emisorListadoGrid.Columns["Nombre"].DataPropertyName = "Nombre";

            emisorListadoGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Aplicar estilos al DataGridView
            StyleDataGridView();

            emisorSearchTextBox.GotFocus += (s, e) =>
            {
                if (emisorSearchTextBox.Text == "Buscar por nombre o #ID")
                {
                    emisorSearchTextBox.Text = "";
                    emisorSearchTextBox.ForeColor = Color.Black;
                }
            };

            emisorSearchTextBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(emisorSearchTextBox.Text))
                {
                    emisorSearchTextBox.Text = "Buscar por nombre o #ID";
                    emisorSearchTextBox.ForeColor = Color.Gray;
                }
            };

            emisorInputNombre.GotFocus += (s, e) =>
            {
                if (emisorInputNombre.Text == "Nombre")
                {
                    emisorInputNombre.Text = "";
                    emisorInputNombre.ForeColor = Color.Black;
                }
            };

            emisorInputNombre.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(emisorInputNombre.Text))
                {
                    emisorInputNombre.Text = "Nombre";
                    emisorInputNombre.ForeColor = Color.Gray;
                }
            };
        }

        private void ConfigurarEventos()
        {
            emisorListadoGrid.SelectionChanged += EmisoresListadoGrid_SelectionChanged;
            emisorSearchTextBox.TextChanged += EmisoresSearchTextBox_TextChanged;
            emisorBotonGuardar.Click += EmisoresBotonGuardar_Click;
            emisorBotonCancelar.Click += EmisoresBotonCancelar_Click;
            botonEstado.Click += BotonEstado_Click;
        }

        private async void CargarEmisoresAsync()
        {
            try
            {
                var emisores = await _emisorService.ObtenerEmisoresAsync();
                emisorListadoGrid.DataSource = emisores;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar emisores: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EmisoresListadoGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (emisorListadoGrid.SelectedRows.Count > 0)
            {
                _emisorSeleccionado = (Emisor)emisorListadoGrid.SelectedRows[0].DataBoundItem;
                CargarDatosEmisor(_emisorSeleccionado);
                _esModoEdicion = true;
                botonEstado.Visible = true;
                ActualizarBotonEstado();
            }
        }

        private void CargarDatosEmisor(Emisor emisor)
        {
            if (emisor != null)
            {
                emisorInputNombre.Text = emisor.Nombre;
                emisorInputNombre.ForeColor = Color.Black;
            }
        }

        private void ActualizarBotonEstado()
        {
            if (_emisorSeleccionado != null)
            {
                botonEstado.Text = _emisorSeleccionado.Activo ? "Desactivar" : "Activar";
                botonEstado.BackColor = _emisorSeleccionado.Activo ? Color.FromArgb(220, 53, 69) : Color.FromArgb(40, 167, 69);
            }
        }

        private async void EmisoresBotonGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(emisorInputNombre.Text) || emisorInputNombre.Text == "Nombre")
                {
                    MessageBox.Show("El nombre del emisor es requerido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_esModoEdicion && _emisorSeleccionado != null)
                {
                    _emisorSeleccionado.Nombre = emisorInputNombre.Text;
                    await _emisorService.ActualizarEmisorAsync(_emisorSeleccionado);
                    MessageBox.Show("Emisor actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var nuevoEmisor = new Emisor
                    {
                        Nombre = emisorInputNombre.Text,
                        Activo = true
                    };
                    await _emisorService.AddEmisorAsync(nuevoEmisor);
                    MessageBox.Show("Emisor creado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LimpiarFormulario();
                CargarEmisoresAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el emisor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EmisoresBotonCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private async void BotonEstado_Click(object sender, EventArgs e)
        {
            if (_emisorSeleccionado == null) return;

            try
            {
                string mensaje = _emisorSeleccionado.Activo
                    ? $"¿Está seguro que desea desactivar el emisor '{_emisorSeleccionado.Nombre}'?"
                    : $"¿Está seguro que desea activar el emisor '{_emisorSeleccionado.Nombre}'?";

                var resultado = MessageBox.Show(mensaje, "Confirmar acción",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    if (_emisorSeleccionado.Activo)
                    {
                        _emisorSeleccionado = await _emisorService.DesactivarEmisorAsync(_emisorSeleccionado);
                    }
                    else
                    {
                        _emisorSeleccionado = await _emisorService.ActivarEmisorAsync(_emisorSeleccionado);
                    }

                    ActualizarBotonEstado();
                    CargarEmisoresAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar el estado del emisor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void EmisoresSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var emisores = await _emisorService.ObtenerEmisoresAsync();
                var textoBusqueda = emisorSearchTextBox.Text.ToLower();

                if (textoBusqueda != "buscar por nombre o #id" && !string.IsNullOrWhiteSpace(textoBusqueda))
                {
                    emisores = emisores.Where(b =>
                        b.Nombre.ToLower().Contains(textoBusqueda) ||
                        b.Id.ToString().Contains(textoBusqueda)
                    ).ToList();
                }

                emisorListadoGrid.DataSource = emisores;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar emisores: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            emisorInputNombre.Text = "Nombre";
            emisorInputNombre.ForeColor = Color.Gray;
            _emisorSeleccionado = null;
            _esModoEdicion = false;
            botonEstado.Visible = false;
            emisorListadoGrid.ClearSelection();
        }

        private void StyleDataGridView()
        {
            emisorListadoGrid.EnableHeadersVisualStyles = false;
            emisorListadoGrid.BorderStyle = BorderStyle.None;
            emisorListadoGrid.BackgroundColor = Color.WhiteSmoke;
            emisorListadoGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 123, 255);
            emisorListadoGrid.DefaultCellStyle.SelectionForeColor = Color.White;
            emisorListadoGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 123, 255);
            emisorListadoGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            emisorListadoGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            emisorListadoGrid.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            emisorListadoGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
        }
    }
}