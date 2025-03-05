using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PuntoDeVenta
{
    public partial class Login : Form
    {

        public event EventHandler<int> ValorEnviado;
        private MySQLHelper mysqlHelper;
        private MySqlConnection conexion;


        public Login()
        {
            InitializeComponent();

            mysqlHelper = new MySQLHelper();
            conexion = mysqlHelper.ObtenerConexion();

            txtUsuario.Focus();
            LoadLogo();


        }

        private void LoadLogo()
        {
            try
            {

                string configPath = @"C:\caja\ConfiguracionCaja.txt";


                string[] configLines = File.ReadAllLines(configPath);


                string logoFileName = string.Empty;
                foreach (string line in configLines)
                {
                    if (line.StartsWith("Logo:"))
                    {
                        logoFileName = line.Substring(5).Trim();
                        break;
                    }
                }


                if (!string.IsNullOrEmpty(logoFileName))
                {

                    string logoPath = Path.Combine(@"C:\caja\", logoFileName);


                    if (File.Exists(logoPath))
                    {

                        pictureBox1.Image = new Bitmap(logoPath);
                    }
                    else
                    {
                        MessageBox.Show($"No se encontró el archivo de logo: {logoPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("No se encontró la configuración del logo en el archivo de configuración.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al cargar el logo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

            txtUsuario.Text = txtUsuario.Text.ToUpper();

            txtUsuario.SelectionStart = txtUsuario.Text.Length;
        }



        public void ValidarAcceso()
        {

            string usuario = txtUsuario.Text;
            string clave = txtClave.Text;

            string consulta = "SELECT * FROM apos03 WHERE d2 = @Usuario AND d3 = @Clave";
            MySqlCommand comando = new MySqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@Usuario", usuario);
            comando.Parameters.AddWithValue("@Clave", clave);
            try
            {
                conexion.Open();
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.Read())
                {

                    String idUsuario = Convert.ToString(reader["d1"]);
                    String nombreUsuario = Convert.ToString(reader["d2"]);

                    Form1 formDestino = new Form1();
                    formDestino.IdUsuario2 = idUsuario;
                    formDestino.NombreUsuario = nombreUsuario;
                    formDestino.Show();

                    this.Close();
                }
                else
                {

                    DialogResult result;
                    result = MessageBox.Show("Usuario o contraseña incorrectos", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.Cancel)
                    {
                        result = MessageBox.Show("Usuario o contraseña incorrectos", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
                        txtUsuario.Focus();
                        txtUsuario.SelectAll();
                    }

                    else if (result == DialogResult.OK)
                    {
                        txtUsuario.Focus();
                        txtUsuario.SelectAll();
                    }


                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar acceder a la base de datos: " + ex.Message);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }
        }

        public void EnviarValor()
        {
            int valor = 10;
            ValorEnviado?.Invoke(this, valor);
        }

        private void txtUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                txtClave.Focus();
                txtClave.SelectAll();

                e.SuppressKeyPress = true;
            }
        }

        private void txtClave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                ValidarAcceso();

                e.SuppressKeyPress = true;
            }

            if (e.KeyCode == Keys.Up)
            {
                txtUsuario.Focus();
                e.Handled = true;
            }
        }


        private void Login_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Escape)
            {

                DialogResult result = MessageBox.Show("¿Desea salir del sistema?", "Confirmar salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


                if (result == DialogResult.Yes)
                {

                    this.Close();
                    Environment.Exit(0);
                }
                else
                {
                    txtUsuario.Focus();
                }
            }

        }


        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ValidarAcceso();
        }


    }
}
