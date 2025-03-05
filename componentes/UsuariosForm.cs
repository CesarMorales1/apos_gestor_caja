using MySql.Data.MySqlClient;
using MyApp.Infrastructure.Database;
using System;
using System.Windows.Forms;

namespace apos_gestor_caja.componentes
{
    public partial class UsuariosForm : Form
    {
        private SqlHelper dbConnection;

        public UsuariosForm()
        {
            dbConnection = new SqlHelper();
            InitializeComponent();
        }

        private void UsuariosForm_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void Nombre_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void cajLabelTitulo_Click(object sender, EventArgs e)
        {

        }

        private void cajInputNombre_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void cajInputNombre_Enter(object sender, EventArgs e)
        {

        }
    }
}