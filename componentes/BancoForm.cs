using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using apos_gestor_caja.Domain.Models;
using apos_gestor_caja.ApplicationLayer.Services;
using System.Linq;
using System.IO;
using apos_gestor_caja.Infrastructure.Repositories;

namespace apos_gestor_caja.Forms
{
    public partial class BancoForm : Form
    {
        private readonly BancoService _bancoService;
        private bool isEditMode = false;
        private int? editingBancoId = null;
        private const string PLACEHOLDER_NOMBRE = "Ingrese el nombre del banco";
        private List<Banco> _allBancos;
        private bool _isUpdating = false;
        private bool _ignoreSelectionChange = false;

        public BancoForm(BancoService bancoService)
        {
            InitializeComponent();
            _bancoService = bancoService ?? throw new ArgumentNullException(nameof(bancoService));

            _allBancos = new List<Banco>();
            _ignoreSelectionChange = false;

            ConfigurarFormulario();
            ConfigurarDataGridView();
            ConfigurarPlaceholders();
            ConfigurarBusquedaComboBox();
            _ = CargarDatosInicialesAsync();
        }

        private async Task CargarDatosInicialesAsync()
        {
            try
            {
                var bancos = await _bancoService.ObtenerBancosAsync();
                _allBancos = bancos ?? new List<Banco>();

                _ignoreSelectionChange = true;

                ActualizarGrilla(_allBancos);
                ActualizarComboBox(_allBancos);

                cajListadoGrid.ClearSelection();
                cajBusquedaCombo.SelectedIndex = -1;

                _ignoreSelectionChange = false;

                cajInputNombre.Text = PLACEHOLDER_NOMBRE;
                cajInputNombre.ForeColor = Color.Gray;
                cajLabelTitulo.Text = "Nuevo Banco";

                isEditMode = false;
                editingBancoId = null;

                ActualizarEstadoBoton(true);
            }
            catch (Exception ex)
            {
                _allBancos = new List<Banco>();
                MessageBox.Show($"Error al cargar datos iniciales: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ActualizarGrilla(_allBancos);
                ActualizarComboBox(_allBancos);
            }
        }

        private void ConfigurarFormulario()
        {
            cajBotonGuardar.Click += CajBotonGuardar_Click;
            cajBotonCancelar.Click += CajBotonCancelar_Click;
            botonEstado.Click += BotonEstado_Click;
        }

        private void ConfigurarBusquedaComboBox()
        {
            cajBusquedaCombo.DropDownStyle = ComboBoxStyle.DropDownList;

            cajBusquedaCombo.Format += (s, e) =>
            {
                if (e.ListItem is Banco banco)
                {
                    e.Value = $"{banco.Id} - {banco.Nombre}";
                }
            };

            cajBusquedaCombo.SelectedIndexChanged += async (s, e) =>
            {
                if (_isUpdating) return;
                _isUpdating = true;

                if (cajBusquedaCombo.SelectedItem is Banco selectedBanco)
                {
                    await CargarBancoParaEdicion(selectedBanco.Id);
                }

                _isUpdating = false;
            };

            cajSearchTextBox.TextChanged += async (s, e) =>
            {
                string searchText = cajSearchTextBox.Text.Trim();
                List<Banco> filteredBancos;

                if (string.IsNullOrWhiteSpace(searchText) || searchText == "Buscar por nombre o #ID")
                {
                    filteredBancos = _allBancos ?? new List<Banco>();
                }
                else if (searchText.StartsWith("#") && int.TryParse(searchText.Substring(1), out int id))
                {
                    filteredBancos = (_allBancos ?? new List<Banco>()).Where(b => b.Id == id).ToList();
                }
                else
                {
                    filteredBancos = (_allBancos ?? new List<Banco>()).Where(b => b.Nombre.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }

                Banco bancoSeleccionado = cajListadoGrid.CurrentRow?.DataBoundItem as Banco;

                _isUpdating = true;
                ActualizarComboBox(filteredBancos);
                ActualizarGrilla(filteredBancos);

                if (bancoSeleccionado != null && filteredBancos.Contains(bancoSeleccionado))
                {
                    int index = filteredBancos.IndexOf(bancoSeleccionado);
                    cajListadoGrid.CurrentCell = cajListadoGrid.Rows[index].Cells[0];
                    cajListadoGrid.Rows[index].Selected = true;
                    cajBusquedaCombo.SelectedItem = bancoSeleccionado;
                }
                else if (filteredBancos.Count > 0)
                {
                    cajListadoGrid.CurrentCell = cajListadoGrid.Rows[0].Cells[0];
                    cajListadoGrid.Rows[0].Selected = true;
                    await CargarBancoParaEdicion(filteredBancos[0].Id);
                }

                _isUpdating = false;
            };
        }

        private async Task CargarBancoParaEdicion(int id)
        {
            try
            {
                var banco = await _bancoService.GetBancoByIdAsync(id);
                if (banco != null)
                {
                    editingBancoId = banco.Id;
                    isEditMode = true;

                    cajInputNombre.Text = banco.Nombre;
                    cajInputNombre.ForeColor = Color.Black;
                    cajInputNombre.Enabled = true;

                    cajLabelTitulo.Text = "Editar Banco";
                    ActualizarEstadoBoton(banco.Activo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el banco: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            var nombreColumn = new DataGridViewTextBoxColumn
            {
                Name = "Nombre",
                DataPropertyName = "Nombre",
                HeaderText = "Nombre",
                Width = 400
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
                nombreColumn,
                activoColumn
            });

            cajListadoGrid.SelectionChanged += async (s, e) =>
            {
                if (_isUpdating || _ignoreSelectionChange) return;

                _isUpdating = true;
                try
                {
                    if (cajListadoGrid.CurrentRow != null)
                    {
                        var banco = cajListadoGrid.CurrentRow.DataBoundItem as Banco;
                        if (banco != null)
                        {
                            await CargarBancoParaEdicion(banco.Id);
                            cajBusquedaCombo.SelectedItem = banco;
                        }
                    }
                }
                finally
                {
                    _isUpdating = false;
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
            if (cajInputNombre.Text == PLACEHOLDER_NOMBRE || string.IsNullOrWhiteSpace(cajInputNombre.Text))
            {
                MessageBox.Show("El nombre del banco es requerido", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cajInputNombre.Focus();
                return false;
            }
            return true;
        }

        private async Task GuardarBanco()
        {
            if (!ValidarFormulario()) return;

            try
            {
                Banco banco = new Banco
                {
                    Id = isEditMode && editingBancoId.HasValue ? editingBancoId.Value : 0,
                    Nombre = cajInputNombre.Text.Trim(),
                    Activo = true
                };

                bool resultado;
                if (isEditMode)
                {
                    if (!editingBancoId.HasValue || editingBancoId <= 0)
                    {
                        MessageBox.Show("No se ha seleccionado un banco válido para editar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    resultado = await _bancoService.ActualizarBancoAsync(banco);
                }
                else
                {
                    resultado = await _bancoService.AddBancoAsync(banco);
                }

                if (resultado)
                {
                    MessageBox.Show("Banco guardado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    await CargarDatosInicialesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el banco: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            cajInputNombre.Text = "";
            ConfigurarPlaceholders();

            isEditMode = false;
            editingBancoId = null;
            cajLabelTitulo.Text = "Nuevo Banco";
            cajInputNombre.Focus();
            cajInputNombre.Enabled = true;

            cajSearchTextBox.Text = "Buscar por nombre o #ID";
            cajSearchTextBox.ForeColor = Color.Gray;

            _ignoreSelectionChange = true;

            ActualizarComboBox(_allBancos ?? new List<Banco>());
            ActualizarGrilla(_allBancos ?? new List<Banco>());
            cajBusquedaCombo.SelectedIndex = -1;
            cajListadoGrid.ClearSelection();

            _ignoreSelectionChange = false;

            ActualizarEstadoBoton(true);
        }

        private void ConfigurarPlaceholders()
        {
            ConfigurarPlaceholder(cajInputNombre, PLACEHOLDER_NOMBRE);
            ConfigurarPlaceholder(cajSearchTextBox, "Buscar por nombre o #ID");
        }

        private void ConfigurarPlaceholder(TextBox textBox, string placeholder)
        {
            if (string.IsNullOrEmpty(textBox.Text) || textBox.Text == placeholder)
            {
                textBox.Text = placeholder;
                textBox.ForeColor = Color.Gray;
            }

            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };

            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                }
            };
        }

        private void ActualizarGrilla(List<Banco> bancos)
        {
            try
            {
                cajListadoGrid.DataSource = null;
                cajListadoGrid.DataSource = new BindingSource { DataSource = bancos };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar la grilla: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarComboBox(List<Banco> bancos)
        {
            cajBusquedaCombo.Items.Clear();
            foreach (var banco in bancos)
            {
                cajBusquedaCombo.Items.Add(banco);
            }
            cajBusquedaCombo.SelectedIndex = -1;
        }

        private void ActualizarEstadoBoton(bool activo)
        {
            if (activo)
            {
                botonEstado.Text = "Desactivar";
                botonEstado.BackColor = Color.FromArgb(220, 60, 90);
            }
            else
            {
                botonEstado.Text = "Activar";
                botonEstado.BackColor = Color.FromArgb(40, 167, 69);
            }
        }

        private async void CajBotonGuardar_Click(object sender, EventArgs e)
        {
            await GuardarBanco();
        }

        private void CajBotonCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private async void BotonEstado_Click(object sender, EventArgs e)
        {
            if (!isEditMode || !editingBancoId.HasValue || editingBancoId <= 0)
            {
                MessageBox.Show("Seleccione un banco válido para cambiar su estado", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var bancoActual = await _bancoService.GetBancoByIdAsync(editingBancoId.Value);
                if (bancoActual == null)
                {
                    MessageBox.Show("El banco no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string accion = bancoActual.Activo ? "desactivar" : "activar";
                var confirmResult = MessageBox.Show($"¿Está seguro de que desea {accion} este banco?",
                    $"Confirmar {accion}", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult != DialogResult.Yes)
                    return;

                Banco banco;
                if (bancoActual.Activo)
                {
                    banco = await _bancoService.DesactivarBancoAsync(bancoActual);
                }
                else
                {
                    banco = await _bancoService.ActivarBancoAsync(bancoActual);
                }

                if (banco != null)
                {
                    MessageBox.Show($"Banco {accion}do exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    await CargarDatosInicialesAsync();
                }
                else
                {
                    MessageBox.Show($"No se pudo {accion} el banco", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar el estado del banco: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BotonEliminar_Click(object sender, EventArgs e)
        {
            if (!isEditMode || !editingBancoId.HasValue || editingBancoId <= 0)
            {
                MessageBox.Show("Seleccione un banco válido para eliminar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var confirmResult = MessageBox.Show("¿Está seguro de que desea eliminar este banco?",
                    "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult != DialogResult.Yes)
                    return;

                var bancoActual = await _bancoService.GetBancoByIdAsync(editingBancoId.Value);
                if (bancoActual == null)
                {
                    MessageBox.Show("El banco no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var resultado = await _bancoService.DesactivarBancoAsync(bancoActual);
                if (resultado != null)
                {
                    MessageBox.Show("Banco eliminado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    await CargarDatosInicialesAsync();
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el banco", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar el banco: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cajBotonGuardar_Click_1(object sender, EventArgs e)
        {

        }
    }
}