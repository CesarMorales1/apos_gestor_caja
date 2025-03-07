using MySql.Data.MySqlClient;
using MyApp.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using apos_gestor_caja.Domain.Models;
using apos_gestor_caja.service;

namespace apos_gestor_caja.componentes
{
    public partial class UsuariosForm : Form
    {
        private readonly UsuarioService _usuarioService;
        private List<Usuario> usuarios;
        private int? editingUsuarioId;

        public UsuariosForm()
        {
            _usuarioService = new UsuarioService();
            InitializeComponent();
            ConfigurarControles();

            // Configure responsive behavior
            this.Load += UsuariosForm_Load;
            this.Resize += UsuariosForm_Resize;
        }

        private async void UsuariosForm_Load(object sender, EventArgs e)
        {
            AjustarTamañoSplitContainer();
            await CargarDatosInicialesAsync();
        }

        private void UsuariosForm_Resize(object sender, EventArgs e)
        {
            AjustarTamañoSplitContainer();
        }

        private void AjustarTamañoSplitContainer()
        {
            // Calculate proportional split based on form width
            int totalWidth = this.ClientSize.Width;
            splitContainer.SplitterDistance = (int)(totalWidth * 0.67); // 67% for the grid panel

            // Adjust search textbox width
            txtBuscar.Width = splitContainer.Panel1.Width - 28; // 14px padding on each side

            // Ensure the grid fills the available space
            dataGridViewUsuarios.Width = splitContainer.Panel1.Width - 28;
            dataGridViewUsuarios.Height = splitContainer.Panel1.Height - dataGridViewUsuarios.Top - 14;

            // Adjust panel controls width
            panelEdicion.Width = splitContainer.Panel2.Width - 18; // 9px padding on each side

            txtNombre.Width = panelEdicion.Width - 48;
            txtUsername.Width = panelEdicion.Width - 48;
            txtPassword.Width = panelEdicion.Width - 48;
            tableLayoutPanel1.Width = panelEdicion.Width - 48;
        }

        private void ConfigurarControles()
        {
            // Configuración inicial del DataGridView
            dataGridViewUsuarios.AutoGenerateColumns = false;
            dataGridViewUsuarios.AllowUserToAddRows = false;
            dataGridViewUsuarios.ReadOnly = true;
            dataGridViewUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsuarios.MultiSelect = false;

            // Agregar columnas
            dataGridViewUsuarios.Columns.Clear();
            dataGridViewUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 50,
                Visible = false
            });

            dataGridViewUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Nombre",
                DataPropertyName = "Nombre",
                HeaderText = "Nombre",
                Width = 150
            });

            dataGridViewUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Username",
                DataPropertyName = "Username",
                HeaderText = "Usuario",
                Width = 100
            });

            dataGridViewUsuarios.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "Activo",
                DataPropertyName = "Activo",
                HeaderText = "Activo",
                Width = 70
            });

            // Botones
            btnNuevo.Text = "Nuevo";
            btnGuardar.Text = "Guardar";
            btnCancelar.Text = "Cancelar";
            btnActivar.Text = "Activar/Desactivar";

            // Etiquetas
            lblTitulo.Text = "Gestión de Usuarios";
            lblNombre.Text = "Nombre:";
            lblUsername.Text = "Usuario:";
            lblPassword.Text = "Contraseña:";

            // Cuadros de texto
            txtNombre.Clear();
            txtUsername.Clear();
            txtPassword.Clear();

            // Estado inicial
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;
            btnActivar.Enabled = false;

            // Configurar evento para selección de filas
            dataGridViewUsuarios.SelectionChanged += DataGridViewUsuarios_SelectionChanged;
        }

        // Remove duplicate method - using the one from responsive design

        private async Task CargarDatosInicialesAsync()
        {
            try
            {
                txtBuscar.Clear();
                usuarios = await _usuarioService.ObtenerUsuariosAsync();
                dataGridViewUsuarios.DataSource = null;
                dataGridViewUsuarios.DataSource = usuarios;
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtUsername.Clear();
            txtPassword.Clear();

            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;
            btnActivar.Enabled = false;

            editingUsuarioId = null;

            // Habilitar/deshabilitar controles según sea necesario
            txtNombre.Enabled = false;
            txtUsername.Enabled = false;
            txtPassword.Enabled = false;
        }

        private void DataGridViewUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            bool haySeleccion = dataGridViewUsuarios.SelectedRows.Count > 0;
            btnActivar.Enabled = haySeleccion;

            if (haySeleccion)
            {
                var usuario = (Usuario)dataGridViewUsuarios.SelectedRows[0].DataBoundItem;
                editingUsuarioId = usuario.Id;

                txtNombre.Text = usuario.Nombre;
                txtUsername.Text = usuario.Username;
                txtPassword.Text = usuario.Password;

                btnActivar.Text = usuario.Activo ? "Desactivar" : "Activar";
            }
        }

        private async void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombreBusqueda = txtBuscar.Text.Trim();
                usuarios = await _usuarioService.BuscarUsuariosPorNombreAsync(nombreBusqueda);

                dataGridViewUsuarios.DataSource = null;
                dataGridViewUsuarios.DataSource = usuarios;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar usuarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();

            txtNombre.Enabled = true;
            txtUsername.Enabled = true;
            txtPassword.Enabled = true;

            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;

            editingUsuarioId = null;
        }

        private async void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtUsername.Text) ||
                    string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Todos los campos son obligatorios", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var usuario = new Usuario
                {
                    Nombre = txtNombre.Text,
                    Username = txtUsername.Text,
                    Password = txtPassword.Text,
                    Activo = true
                };

                bool success;

                if (editingUsuarioId.HasValue)
                {
                    usuario.Id = editingUsuarioId.Value;
                    success = await _usuarioService.ActualizarUsuarioAsync(usuario);

                    if (success)
                    {
                        MessageBox.Show("Usuario actualizado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // Verificar si el usuario ya existe
                    bool usuarioExiste = await _usuarioService.VerificarUsuarioExistenteAsync(usuario.Username);
                    if (usuarioExiste)
                    {
                        MessageBox.Show("El nombre de usuario ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    success = await _usuarioService.AddUsuarioAsync(usuario);

                    if (success)
                    {
                        MessageBox.Show("Usuario creado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                if (success)
                {
                    LimpiarFormulario();
                    await CargarDatosInicialesAsync();
                }
                else
                {
                    MessageBox.Show("No se pudo guardar el usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private async void BtnActivar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editingUsuarioId.HasValue)
                {
                    MessageBox.Show("No se ha seleccionado un usuario", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var usuarioActual = usuarios.Find(c => c.Id == editingUsuarioId.Value);
                if (usuarioActual == null)
                {
                    MessageBox.Show("No se encontró el usuario seleccionado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string accion = usuarioActual.Activo ? "desactivar" : "activar";
                var confirmResult = MessageBox.Show($"¿Está seguro de que desea {accion} este usuario?",
                    $"Confirmar {accion}", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult != DialogResult.Yes)
                    return;

                Usuario usuario;
                if (usuarioActual.Activo)
                {
                    usuario = await _usuarioService.DesactivarUsuarioPorIdAsync(editingUsuarioId.Value);
                }
                else
                {
                    usuario = await _usuarioService.ActivarUsuarioPorIdAsync(editingUsuarioId.Value);
                }

                if (usuario != null)
                {
                    MessageBox.Show($"Usuario {accion}do exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    await CargarDatosInicialesAsync();
                }
                else
                {
                    MessageBox.Show($"No se pudo {accion} el usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar el estado del usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                BtnBuscar_Click(sender, e);
            }
        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}