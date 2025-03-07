using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using apos_gestor_caja.Domain.Models;
using apos_gestor_caja.ApplicationLayer.Services;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;

namespace apos_gestor_caja.Forms
{
    public partial class CajeroForm : Form
    {
        private readonly CajeroService _cajeroService;
        private bool isEditMode = false;
        private int? editingCajeroId = null;
        private const string PLACEHOLDER_USUARIO = "Ingrese el usuario";
        private const string PLACEHOLDER_CLAVE = "Ingrese la clave";
        private const string PLACEHOLDER_NOMBRE = "Ingrese el nombre";
        private const string PLACEHOLDER_BARRA = "Ingrese la barra (mínimo 6 caracteres)"; // Added placeholder for Barra
        private List<Cajero> _allCajeros;
        private bool _isUpdating = false;
        private bool _isPasswordVisible = false;
        private bool _ignoreSelectionChange = false;

        public CajeroForm()
        {
            InitializeComponent();
            _cajeroService = new CajeroService();

            _allCajeros = new List<Cajero>();
            _ignoreSelectionChange = false;  // Asegurarse de que empiece en false

            // Eliminamos la creación del PictureBox redundante y usamos el botón del designer
            closedEye.BackgroundImage = ByteArrayToImage(Resource1.ClosedEye);
            closedEye.Click += Closed_eye_click;

            ConfigurarFormulario();
            ConfigurarDataGridView();
            ConfigurarPlaceholders();
            ConfigurarBusquedaComboBox();
            _ = CargarDatosInicialesAsync();

            // Configure responsive behavior
            this.Load += CajeroForm_Load;
            this.Resize += CajeroForm_Resize;
        }

        private void CajeroForm_Load(object sender, EventArgs e)
        {
            AjustarTamañoSplitContainer();
        }

        private void CajeroForm_Resize(object sender, EventArgs e)
        {
            AjustarTamañoSplitContainer();
        }

        private void AjustarTamañoSplitContainer()
        {
            // Calculate proportional split based on form width
            int totalWidth = this.ClientSize.Width;
            splitContainer.SplitterDistance = (int)(totalWidth * 0.67); // 67% for the grid panel

            // Adjust search textbox width
            cajSearchTextBox.Width = splitContainer.Panel1.Width - 28; // 14px padding on each side

            // Ensure the grid fills the available space
            cajListadoGrid.Width = splitContainer.Panel1.Width - 28;
            cajListadoGrid.Height = splitContainer.Panel1.Height - cajListadoGrid.Top - 14;

            // Adjust panel controls width
            cajPanelEdicion.Width = splitContainer.Panel2.Width - 18; // 9px padding on each side

            cajInputUsuario.Width = cajPanelEdicion.Width - 34;
            cajInputClave.Width = cajPanelEdicion.Width - 34;
            cajInputNombre.Width = cajPanelEdicion.Width - 34;
            cajComboNivel.Width = cajPanelEdicion.Width - 34;
            textBox1.Width = cajPanelEdicion.Width - 34;

            // Reposition eye icon - usar el botón closedEye del designer
            closedEye.Location = new Point(cajInputClave.Right - 42, cajInputClave.Location.Y);
        }

        private async Task CargarDatosInicialesAsync()
        {
            try
            {
                var cajeros = await _cajeroService.ObtenerCajerosAsync();
                _allCajeros = cajeros ?? new List<Cajero>();

                _ignoreSelectionChange = true;

                ActualizarGrilla(_allCajeros);
                ActualizarComboBox(_allCajeros);

                cajListadoGrid.ClearSelection(); // Mantener sin selección al inicio
                cajBusquedaCombo.SelectedIndex = -1;

                _ignoreSelectionChange = false;

                cajInputUsuario.Text = PLACEHOLDER_USUARIO;
                cajInputUsuario.ForeColor = Color.Gray;

                cajInputClave.Text = PLACEHOLDER_CLAVE;
                cajInputClave.ForeColor = Color.Gray;
                cajInputClave.PasswordChar = '*';

                cajInputNombre.Text = PLACEHOLDER_NOMBRE;
                cajInputNombre.ForeColor = Color.Gray;

                textBox1.Text = PLACEHOLDER_BARRA;
                textBox1.ForeColor = Color.Gray;
                UpdateBarraTextBoxState();

                cajComboNivel.SelectedIndex = 0;
                cajLabelTitulo.Text = "Nuevo Cajero";

                isEditMode = false;
                editingCajeroId = null;
            }
            catch (Exception ex)
            {
                _allCajeros = new List<Cajero>();
                MessageBox.Show($"Error al cargar datos iniciales: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ActualizarGrilla(_allCajeros);
                ActualizarComboBox(_allCajeros);
            }
        }

        private void ConfigurarFormulario()
        {
            cajComboNivel.Items.AddRange(new object[] { 5, 9 });
            cajComboNivel.SelectedIndex = 0;
            UpdateBarraTextBoxState();

            cajBotonGuardar.Click += CajBotonGuardar_Click;
            cajBotonCancelar.Click += CajBotonCancelar_Click;
            botonEstado.Click += BotonEstado_Click; // Vincular el nuevo evento
            cajComboNivel.SelectedIndexChanged += CajComboNivel_SelectedIndexChanged;
        }

        private void ConfigurarBusquedaComboBox()
        {
            cajBusquedaCombo.DropDownStyle = ComboBoxStyle.DropDownList;

            cajBusquedaCombo.Format += (s, e) =>
            {
                if (e.ListItem is Cajero cajero)
                {
                    e.Value = $"{cajero.Id} - {cajero.Usuario} ({cajero.Nombre})";
                }
            };

            cajBusquedaCombo.SelectedIndexChanged += async (s, e) =>
            {
                if (_isUpdating) return;
                _isUpdating = true;

                if (cajBusquedaCombo.SelectedItem is Cajero selectedCajero)
                {
                    await CargarCajeroParaEdicion(selectedCajero.Id);
                }

                _isUpdating = false;
            };

            cajSearchTextBox.TextChanged += async (s, e) =>
            {
                string searchText = cajSearchTextBox.Text.Trim();
                List<Cajero> filteredCajeros;

                if (string.IsNullOrWhiteSpace(searchText) || searchText == "Buscar por usuario o #ID")
                {
                    filteredCajeros = _allCajeros ?? new List<Cajero>();
                }
                else if (searchText.StartsWith("#") && int.TryParse(searchText.Substring(1), out int id))
                {
                    filteredCajeros = (_allCajeros ?? new List<Cajero>()).Where(c => c.Id == id).ToList();
                }
                else
                {
                    filteredCajeros = (_allCajeros ?? new List<Cajero>()).Where(c => c.Usuario.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }

                // Guardar el cajero seleccionado actual (si existe)
                Cajero cajeroSeleccionado = cajListadoGrid.CurrentRow?.DataBoundItem as Cajero;

                _isUpdating = true; // Evitar que SelectionChanged interfiera
                ActualizarComboBox(filteredCajeros);
                ActualizarGrilla(filteredCajeros);

                // Restaurar la selección si el cajero sigue en la lista filtrada
                if (cajeroSeleccionado != null && filteredCajeros.Contains(cajeroSeleccionado))
                {
                    int index = filteredCajeros.IndexOf(cajeroSeleccionado);
                    cajListadoGrid.CurrentCell = cajListadoGrid.Rows[index].Cells[0];
                    cajListadoGrid.Rows[index].Selected = true;
                    cajBusquedaCombo.SelectedItem = cajeroSeleccionado;
                }
                else if (filteredCajeros.Count > 0)
                {
                    // Si no hay selección previa válida, seleccionar el primero de la lista filtrada
                    cajListadoGrid.CurrentCell = cajListadoGrid.Rows[0].Cells[0];
                    cajListadoGrid.Rows[0].Selected = true;
                    await CargarCajeroParaEdicion(filteredCajeros[0].Id);
                }

                _isUpdating = false;
            };
        }

        private async Task CargarCajeroParaEdicion(int id)
        {
            try
            {
                var cajero = await _cajeroService.ObtenerCajeroPorIdAsync(id);
                if (cajero != null)
                {
                    editingCajeroId = cajero.Id;
                    isEditMode = true;

                    cajInputUsuario.Text = cajero.Usuario;
                    cajInputUsuario.ForeColor = Color.Black;

                    cajInputClave.Text = cajero.Clave;
                    cajInputClave.ForeColor = Color.Black;

                    cajInputNombre.Text = cajero.Nombre;
                    cajInputNombre.ForeColor = Color.Black;

                    cajComboNivel.SelectedItem = cajero.NivelAcceso;
                    UpdateBarraTextBoxState();

                    if (cajero.NivelAcceso == 9)
                    {
                        textBox1.Text = cajero.Barra.ToString();
                        textBox1.ForeColor = Color.Black;
                    }
                    else
                    {
                        textBox1.Text = cajero.Barra.ToString();
                        textBox1.ForeColor = Color.Gray;
                    }

                    cajLabelTitulo.Text = "Editar Cajero";

                    _isPasswordVisible = false;
                    cajInputClave.PasswordChar = '*';
                    closedEye.BackgroundImage = ByteArrayToImage(Resource1.ClosedEye);
                    closedEye.Refresh();

                    // Actualizar el estado del botón según Activo
                    ActualizarEstadoBoton(cajero.Activo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el cajero: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CajBotonNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private async void CajBotonGuardar_Click(object sender, EventArgs e)
        {
            await GuardarCajero();
        }

        private void ConfigurarDataGridView()
        {
            cajListadoGrid.AutoGenerateColumns = false;
            cajListadoGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            cajListadoGrid.MultiSelect = false;
            cajListadoGrid.ReadOnly = true;
            cajListadoGrid.AllowUserToAddRows = false;
            cajListadoGrid.RowHeadersVisible = false;
            cajListadoGrid.Columns.Clear();

            var idColumn = new DataGridViewTextBoxColumn
            {
                Name = "Id",
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 60
            };

            var usuarioColumn = new DataGridViewTextBoxColumn
            {
                Name = "Usuario",
                DataPropertyName = "Usuario",
                HeaderText = "Usuario",
                Width = 120
            };

            var nombreColumn = new DataGridViewTextBoxColumn
            {
                Name = "Nombre",
                DataPropertyName = "Nombre",
                HeaderText = "Nombre",
                Width = 200
            };

            var nivelColumn = new DataGridViewTextBoxColumn
            {
                Name = "NivelAcceso",
                DataPropertyName = "NivelAcceso",
                HeaderText = "Nivel",
                Width = 80
            };

            var barraColumn = new DataGridViewTextBoxColumn
            {
                Name = "Barra",
                DataPropertyName = "Barra",
                HeaderText = "Barra",
                Width = 80
            };

            var activoColumn = new DataGridViewCheckBoxColumn
            {
                Name = "Activo",
                DataPropertyName = "Activo",
                HeaderText = "Activo",
                Width = 70
            };

            cajListadoGrid.Columns.AddRange(new DataGridViewColumn[]
            {
        idColumn,
        usuarioColumn,
        nombreColumn,
        nivelColumn,
        barraColumn,
        activoColumn // Nueva columna "Activo"
            });

            cajListadoGrid.SelectionChanged += async (s, e) =>
            {
                if (_isUpdating || _ignoreSelectionChange) return;

                _isUpdating = true;
                try
                {
                    if (cajListadoGrid.CurrentRow != null)
                    {
                        var cajero = cajListadoGrid.CurrentRow.DataBoundItem as Cajero;
                        if (cajero != null)
                        {
                            await CargarCajeroParaEdicion(cajero.Id);
                            cajBusquedaCombo.SelectedItem = cajero; // Sincronizar ComboBox
                        }
                    }
                }
                finally
                {
                    _isUpdating = false; // Asegurar que siempre se restablezca
                }
            };

            StyleDataGridView();
        }

        private void StyleDataGridView()
        {
            cajListadoGrid.EnableHeadersVisualStyles = false;
            cajListadoGrid.BorderStyle = BorderStyle.None;
            cajListadoGrid.BackgroundColor = Color.WhiteSmoke;
            cajListadoGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 123, 255);
            cajListadoGrid.DefaultCellStyle.SelectionForeColor = Color.White;
            cajListadoGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 123, 255);
            cajListadoGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            cajListadoGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            cajListadoGrid.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            cajListadoGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
        }

        private bool ValidarFormulario()
        {
            if (cajInputUsuario.Text == PLACEHOLDER_USUARIO || string.IsNullOrWhiteSpace(cajInputUsuario.Text))
            {
                MessageBox.Show("El usuario es requerido", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cajInputUsuario.Focus();
                return false;
            }

            if (!isEditMode && (cajInputClave.Text == PLACEHOLDER_CLAVE || string.IsNullOrWhiteSpace(cajInputClave.Text)))
            {
                MessageBox.Show("La clave es requerida", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cajInputClave.Focus();
                return false;
            }

            if (cajInputNombre.Text == PLACEHOLDER_NOMBRE || string.IsNullOrWhiteSpace(cajInputNombre.Text))
            {
                MessageBox.Show("El nombre es requerido", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cajInputNombre.Focus();
                return false;
            }

            if (cajComboNivel.SelectedItem != null && Convert.ToInt32(cajComboNivel.SelectedItem) == 9)
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text) || textBox1.Text == PLACEHOLDER_BARRA)
                {
                    MessageBox.Show("La barra es requerida para nivel 9", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox1.Focus();
                    return false;
                }

                if (!int.TryParse(textBox1.Text, out int barraValue) || barraValue < 6)
                {
                    MessageBox.Show("La barra debe ser un número mayor o igual a 6", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox1.Focus();
                    return false;
                }
            }

            return true;
        }

        private async Task GuardarCajero()
        {
            if (!ValidarFormulario()) return;

            try
            {
                //Console.WriteLine($"El valor de la barra para el cajero a crear es de {textBox1.Text.Trim()}");

                Cajero cajero = new Cajero
                {
                    Id = editingCajeroId ?? 0,
                    Usuario = cajInputUsuario.Text.Trim(),
                    Clave = cajInputClave.Text == PLACEHOLDER_CLAVE ? "" : cajInputClave.Text,
                    Nombre = cajInputNombre.Text.Trim(),
                    NivelAcceso = Convert.ToInt32(cajComboNivel.SelectedItem),
                    Activo = true,
                    Barra = (cajComboNivel.SelectedItem != null && Convert.ToInt32(cajComboNivel.SelectedItem) == 9 && int.TryParse(textBox1.Text.Trim(), out int barraValue))
                        ? barraValue
                        : 0 // Si no es nivel 9 o no es un número válido, Barra será 0
                };

                bool resultado;
                if (isEditMode)
                    resultado = await _cajeroService.ActualizarCajeroAsync(cajero);
                else
                    resultado = await _cajeroService.CrearCajeroAsync(cajero);

                if (resultado)
                {
                    MessageBox.Show("Cajero guardado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    await CargarDatosInicialesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el cajero: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            cajInputUsuario.Text = "";
            cajInputClave.Text = "";
            cajInputNombre.Text = "";
            textBox1.Text = "";

            ConfigurarPlaceholders();

            cajComboNivel.SelectedIndex = 0;
            UpdateBarraTextBoxState();

            isEditMode = false;
            editingCajeroId = null;
            cajLabelTitulo.Text = "Nuevo Cajero";
            cajInputUsuario.Focus();

            cajSearchTextBox.Text = "Buscar por usuario o #ID";
            cajSearchTextBox.ForeColor = Color.Gray;

            _ignoreSelectionChange = true;

            ActualizarComboBox(_allCajeros ?? new List<Cajero>());
            ActualizarGrilla(_allCajeros ?? new List<Cajero>());
            cajBusquedaCombo.SelectedIndex = -1;
            cajListadoGrid.ClearSelection();

            _ignoreSelectionChange = false;

            _isPasswordVisible = false;
            cajInputClave.PasswordChar = '*';
            closedEye.BackgroundImage = ByteArrayToImage(Resource1.ClosedEye);
            closedEye.Refresh();

            // Restablecer el botón a "Desactivar" para un nuevo cajero
            ActualizarEstadoBoton(true);
        }

        private void ConfigurarPlaceholders()
        {
            ConfigurarPlaceholder(cajInputUsuario, PLACEHOLDER_USUARIO);
            ConfigurarPlaceholder(cajInputClave, PLACEHOLDER_CLAVE, true);
            ConfigurarPlaceholder(cajInputNombre, PLACEHOLDER_NOMBRE);
            ConfigurarPlaceholder(cajSearchTextBox, "Buscar por usuario o #ID");
            ConfigurarPlaceholder(textBox1, PLACEHOLDER_BARRA); // Placeholder for TextBox1
        }

        private void ConfigurarPlaceholder(TextBox textBox, string placeholder, bool isPassword = false)
        {
            if (string.IsNullOrEmpty(textBox.Text) || textBox.Text == placeholder)
            {
                textBox.Text = placeholder;
                textBox.ForeColor = Color.Gray;
                if (isPassword) textBox.PasswordChar = '*';
            }

            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                    if (isPassword) textBox.PasswordChar = _isPasswordVisible ? '\0' : '*';
                }
            };

            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                    if (isPassword) textBox.PasswordChar = '*';
                }
            };
        }

        private void ActualizarGrilla(List<Cajero> cajeros)
        {
            try
            {
                cajListadoGrid.DataSource = null;
                cajListadoGrid.DataSource = new BindingSource { DataSource = cajeros };
                // No limpiar la selección aquí; lo manejamos en el evento TextChanged
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar la grilla: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarComboBox(List<Cajero> cajeros)
        {
            cajBusquedaCombo.Items.Clear();
            foreach (var cajero in cajeros)
            {
                cajBusquedaCombo.Items.Add(cajero);
            }
            cajBusquedaCombo.SelectedIndex = -1;
        }

        private void Closed_eye_click(object sender, EventArgs e)
        {
            _isPasswordVisible = !_isPasswordVisible;

            if (_isPasswordVisible)
            {
                cajInputClave.PasswordChar = '\0';
                this.closedEye.BackgroundImage = Properties.Resources.Eye;
            }
            else
            {
                cajInputClave.PasswordChar = '*';
                this.closedEye.BackgroundImage = Properties.Resources.Closed_Eye;
            }
            closedEye.Refresh();
        }

        private Image ByteArrayToImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0) return null;
            using (var ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }

        private void cajListadoGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CajBotonCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void CajComboNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateBarraTextBoxState();
        }

        private void UpdateBarraTextBoxState()
        {
            if (cajComboNivel.SelectedItem != null)
            {
                int selectedLevel = Convert.ToInt32(cajComboNivel.SelectedItem);
                if (selectedLevel == 5)
                {
                    textBox1.Text = "0"; // Mostrar 0 para nivel 5
                    textBox1.ReadOnly = true;
                    textBox1.ForeColor = Color.Gray;
                }
                else if (selectedLevel == 9)
                {
                    textBox1.ReadOnly = false;
                    if (textBox1.Text == "0" || string.IsNullOrWhiteSpace(textBox1.Text) || textBox1.Text == PLACEHOLDER_BARRA)
                    {
                        textBox1.Text = PLACEHOLDER_BARRA; // Mostrar placeholder si está vacío o es 0
                        textBox1.ForeColor = Color.Gray;
                    }
                    // Si ya tiene un valor válido, no lo cambiamos
                }
            }
            else
            {
                textBox1.ReadOnly = false;
                textBox1.Text = PLACEHOLDER_BARRA;
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void ActualizarEstadoBoton(bool activo)
        {
            if (activo)
            {
                botonEstado.Text = "Desactivar";
                botonEstado.BackColor = Color.FromArgb(220, 60, 90); // Color original (rojizo)
            }
            else
            {
                botonEstado.Text = "Activar";
                botonEstado.BackColor = Color.FromArgb(40, 167, 69); // Verde claro
            }
        }

        private void cajBotonNuevo_Click_1(object sender, EventArgs e)
        {

        }

        private async void BotonEstado_Click(object sender, EventArgs e)
        {
            if (!isEditMode || editingCajeroId == null)
            {
                MessageBox.Show("Seleccione un cajero para cambiar su estado", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var cajeroActual = await _cajeroService.ObtenerCajeroPorIdAsync(editingCajeroId.Value);
                if (cajeroActual == null)
                {
                    MessageBox.Show("El cajero no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string accion = cajeroActual.Activo ? "desactivar" : "activar";
                var confirmResult = MessageBox.Show($"¿Está seguro de que desea {accion} este cajero?",
                    $"Confirmar {accion}", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult != DialogResult.Yes)
                    return;

                Cajero cajero;
                if (cajeroActual.Activo)
                {
                    cajero = await _cajeroService.DesactivarCajeroPorIdAsync(editingCajeroId.Value);
                }
                else
                {
                    cajero = await _cajeroService.ActivarCajeroPorIdAsync(editingCajeroId.Value);
                }

                if (cajero != null)
                {
                    MessageBox.Show($"Cajero {accion}do exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    await CargarDatosInicialesAsync();
                }
                else
                {
                    MessageBox.Show($"No se pudo {accion} el cajero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar el estado del cajero: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BotonEstado_click(object sender, EventArgs e)
        {

        }
    }
}