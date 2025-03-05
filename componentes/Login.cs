using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MyApp.Infrastructure.Database;
using System.Drawing.Drawing2D;
using apos_gestor_caja.Infrastructure.Repositories;
using apos_gestor_caja.Domain.Models;
using System.Threading.Tasks;
using apos_gestor_caja.service;

namespace apos_gestor_caja.componentes
{
    public partial class Login : Form
    {
        private SqlHelper dbConecction;
        private readonly UsuarioRepository usuarioRepository;
        private bool isProcessingEnter = false;

        public Login()
        {
            InitializeComponent();
            this.dbConecction = new SqlHelper();
            this.usuarioRepository = new UsuarioRepository();
            this.KeyUp += new KeyEventHandler(this.Login_keyup);
            this.KeyDown += new KeyEventHandler(this.Login_KeyDown);
            this.KeyPreview = true;
        }

        private void Login_keyup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !isProcessingEnter)
            {
                isProcessingEnter = true;

                try
                {
                    this.loginButton.PerformClick();
                }
                finally
                {
                    Task.Delay(300).ContinueWith(t =>
                    {
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke(new Action(() => { isProcessingEnter = false; }));
                        }
                        else
                        {
                            isProcessingEnter = false;
                        }
                    });
                }

                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void pictureBox1_Click(object sender, EventArgs e) { }

        private void pictureBox3_Click(object sender, EventArgs e) { }

        private void Login_Load(object sender, EventArgs e) { }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            string usuario = this.usuario.Text;
            string password = this.password.Text;

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
            {
                isProcessingEnter = true;
                DialogResult result = MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    this.usuario.Focus();
                }
                Task.Delay(500).ContinueWith(t =>
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new Action(() => { isProcessingEnter = false; }));
                    }
                    else
                    {
                        isProcessingEnter = false;
                    }
                });
                return;
            }

            // Usar el método del repositorio para validar el usuario
            Usuario usuarioEncontrado = await LoginUsuario(usuario, password);
            if (usuarioEncontrado != null)
            {
                principal vistaPrincipal = new principal();
                vistaPrincipal.Show();
                this.Hide();
            }
            else
            {
                isProcessingEnter = true;
                MessageBox.Show("Usuario o contraseña incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.usuario.Focus();

                Task.Delay(500).ContinueWith(t =>
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new Action(() => { isProcessingEnter = false; }));
                    }
                    else
                    {
                        isProcessingEnter = false;
                    }
                });
            }
        }

        private async Task<Usuario> LoginUsuario(string usuario, string password)
        {
            try
            {
                //creando servicios para saber la auditoria
                Usuario loginUser = await usuarioRepository.ObtenerUsuario(usuario, password);
                //guardando informacion en el servicio
                UserInformation.SetUsuario(loginUser);
                return loginUser;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void password_TextChanged(object sender, EventArgs e) { }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int cornerRadius = 10;
            int borderWidth = 3;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, cornerRadius * 2, cornerRadius * 2, 180, 90);
                path.AddArc(ClientSize.Width - cornerRadius * 2 - 1, 0, cornerRadius * 2, cornerRadius * 2, 270, 90);
                path.AddArc(ClientSize.Width - cornerRadius * 2 - 1, ClientSize.Height - cornerRadius * 2 - 1, cornerRadius * 2, cornerRadius * 2, 0, 90);
                path.AddArc(0, ClientSize.Height - cornerRadius * 2 - 1, cornerRadius * 2, cornerRadius * 2, 90, 90);
                path.CloseFigure();

                this.Region = new Region(path);
            }

            using (GraphicsPath borderPath = new GraphicsPath())
            using (Pen borderPen = new Pen(Color.FromArgb(0, 53, 84), borderWidth))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                int halfBorder = borderWidth / 2;
                borderPath.AddArc(halfBorder, halfBorder, cornerRadius * 2, cornerRadius * 2, 180, 90);
                borderPath.AddArc(ClientSize.Width - cornerRadius * 2 - 1 - halfBorder, halfBorder, cornerRadius * 2, cornerRadius * 2, 270, 90);
                borderPath.AddArc(ClientSize.Width - cornerRadius * 2 - 1 - halfBorder, ClientSize.Height - cornerRadius * 2 - 1 - halfBorder, cornerRadius * 2, cornerRadius * 2, 0, 90);
                borderPath.AddArc(halfBorder, ClientSize.Height - cornerRadius * 2 - 1 - halfBorder, cornerRadius * 2, cornerRadius * 2, 90, 90);
                borderPath.CloseFigure();

                e.Graphics.DrawPath(borderPen, borderPath);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate();
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult result = MessageBox.Show("¿Seguro que quiere cerrar el gestor de caja?", "Confirmar salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
                else
                {
                    usuario.Focus();
                }
                e.Handled = true;
            }
        }
    }
}