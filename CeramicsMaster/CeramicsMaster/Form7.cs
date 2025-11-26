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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CeramicsMaster
{
    public partial class Form7 : Form
    {
        private SqlConnection connection = new SqlConnection(@"Server=adclg1;Database=Черноусова_22;Integrated Security=True;TrustServerCertificate=true");
        private string name_prod = "";
        public Form7(string name_prdct)
        {
            this.name_prod = name_prdct;
            InitializeComponent();
            fill_the_list();
            fill_the_space();
        }

        private void fill_the_list()
        {
            connection.Open();
            string str_com = "Select Type_of_products from Product_type_import";
            SqlCommand cmnd = new SqlCommand(str_com, connection);
            SqlDataReader rdr = cmnd.ExecuteReader();
            string znch = "";

            while (rdr.Read())
            {
                znch = rdr.GetString(0);
                comboBox1.Items.Add(znch);
            }
            connection.Close();
        }
        private void fill_the_space()
        {
            connection.Open();
            string str_com = $"Select * from Products_import where Product_name = '{name_prod}'";
            SqlCommand cmnd = new SqlCommand(str_com, connection);
            SqlDataReader rdr = cmnd.ExecuteReader();

            while (rdr.Read())
            {
                comboBox1.Text = rdr.GetString(0);
                textBox2.Text = rdr.GetString(1);
                textBox3.Text = rdr.GetInt32(2).ToString();
                textBox4.Text = Math.Round(rdr.GetDecimal(3), 2).ToString().Replace(',', '.');
                textBox5.Text = Math.Round(rdr.GetDecimal(4), 2).ToString().Replace(',', '.');
            }
            connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            string str_com = $"update Products_import set " +
                $"Product_type = '{comboBox1.Text}', " +
                $"Article = {textBox3.Text}, " +
                $"Minimum_price_for_partner = {textBox4.Text}," +
                $"Roll_width_m = {textBox5.Text}" +
                $"where Product_name = '{name_prod}'";
            SqlCommand cmnd = new SqlCommand(str_com, connection);
            cmnd.ExecuteNonQuery();
            connection.Close();

            MessageBox.Show("Данные успешно сохранены", "Успешно сохранено", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            this.Close();
        }

        private void delete_prod()
        {
            connection.Open();
            string str_com = $"Delete from Product_materials_import where Products = '{name_prod}'";
            SqlCommand cmnd2 = new SqlCommand(str_com, connection);
            cmnd2.ExecuteNonQuery();
            connection.Close();


            connection.Open();
            str_com = $"Delete from Products_import where Product_name = '{name_prod}'";
            SqlCommand cmnd = new SqlCommand(str_com, connection);
            cmnd.ExecuteNonQuery();
            connection.Close();


            MessageBox.Show("Данные успешно Удалены", "Успешно удалено", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Точно удалить товар (после подтверждения действие отменить нельзя)", "Подтвредите удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if (res == DialogResult.Yes)
            {
                delete_prod();
            }
        }
    }
}
