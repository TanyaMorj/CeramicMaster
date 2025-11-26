using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CeramicsMaster
{
    public partial class Form8 : Form
    {
        private SqlConnection connection = new SqlConnection(@"Server=adclg1;Database=Черноусова_22;Integrated Security=True;TrustServerCertificate=true");
        public Form8()
        {
            InitializeComponent();
            fill_the_list();
        }

        private void fill_the_list()
        {
            connection.Open();
            string str_com = "Select Type_of_products from Product_type_import";
            SqlCommand cmnd = new SqlCommand(str_com, connection);
            SqlDataReader rdr = cmnd.ExecuteReader();

            while (rdr.Read())
            {
                comboBox1.Items.Add(rdr.GetString(0));
            }
            connection.Close();

            connection.Open();
            str_com = "Select Material_name from Materials_import";
            SqlCommand cmnd2 = new SqlCommand(str_com, connection);
            SqlDataReader rdr2 = cmnd2.ExecuteReader();

            while (rdr2.Read())
            {
                comboBox2.Items.Add(rdr2.GetString(0));
            }
            connection.Close();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool isDigit = char.IsDigit(e.KeyChar);
            bool isComma = e.KeyChar == '.';
            bool isControl = char.IsControl(e.KeyChar);

            if (!isDigit && !isComma && !isControl)
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            string str_com = $"insert into Products_import values ('{comboBox1.Text}', '{textBox2.Text}', {textBox3.Text}, {textBox4.Text}, {textBox5.Text});";
            SqlCommand cmnd = new SqlCommand(str_com, connection);
            cmnd.ExecuteNonQuery();

            connection.Close();

            connection.Open();
            str_com = $"insert into Product_materials_import values ('{textBox2.Text}', '{comboBox2.Text}', {textBox1.Text});";
            SqlCommand cmnd2 = new SqlCommand(str_com, connection);
            cmnd2.ExecuteNonQuery();

            connection.Close();

            MessageBox.Show("Данные успешно Добавлены", "Успешно добавлено", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            this.Close();
        }
    }
}
