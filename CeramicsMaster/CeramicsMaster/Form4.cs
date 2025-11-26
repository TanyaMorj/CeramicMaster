using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CeramicsMaster
{
    public partial class Form4 : Form
    {
        private SqlConnection connection = new SqlConnection(@"Server=adclg1;Database=Черноусова_22;Integrated Security=True;TrustServerCertificate=true");
        public Form4()
        {
            InitializeComponent();
            fill_the_table();
        }

        private void fill_the_table()
        {
            connection.Open();
            dataGridView1.Columns.Add("logn", "Логин");
            dataGridView1.Columns.Add("dat", "дата входа");
            dataGridView1.Columns.Add("succes", "Успешный вход");

            string str_com = "Select * from History";
            SqlCommand cmnd = new SqlCommand(str_com, connection);
            SqlDataReader rdr = cmnd.ExecuteReader();

            while (rdr.Read())
            {
                dataGridView1.Rows.Add(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2));

            }
            connection.Close();

        }
    }
}
