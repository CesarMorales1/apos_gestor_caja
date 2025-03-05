using System;
using System.Windows.Forms;
using System.Data;
using MyApp.Infrastructure.Database;
using MySql.Data.MySqlClient;

namespace apos_gestor_caja.componentes
{
    public partial class GenerarLibroVentasForm : Form
    {
        private SqlHelper sqlHelper = new SqlHelper();

        public GenerarLibroVentasForm()
        {
            InitializeComponent();
        }

        public DataTable GetVentasData()
        {
            DateTime startDate = startDatePicker.Value.Date;
            DateTime endDate = endDatePicker.Value.Date;

            using (var connection = sqlHelper.ObtenerConexion())
            {
                string query = @"SELECT * FROM paramillo 
                                 WHERE fecha BETWEEN @startDate AND @endDate 
                                 ORDER BY fecha, numero_operacion";

                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);

                    var adapter = new MySqlDataAdapter(cmd);
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}