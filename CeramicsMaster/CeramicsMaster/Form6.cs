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
    public partial class Form6: Form
    {
        private SqlConnection connection = new SqlConnection(@"Server=adclg1;Database=Черноусова_22;Integrated Security=True;TrustServerCertificate=true");
        string login = "";
        public Form6(string login)
        {
            this.login = login;
            InitializeComponent();

            fill_the_space();
        }

        private void fill_the_space()
        {
            string fio = "";
            string rol = "";
            string img = "";
            connection.Open();
            string str_com = $"Select FIO, Role, Image from users where logn = '{login}'";
            SqlCommand cmnd = new SqlCommand(str_com, connection);
            SqlDataReader rdr = cmnd.ExecuteReader();


            while (rdr.Read())
            {
                fio = rdr.GetString(0);
                rol = rdr.GetString(1);
                img = rdr.GetString(2);
            }
            connection.Close();
            pictureBox1.Image = Image.FromFile(img);
            label1.Text = fio;
            label2.Text = login;
            label3.Text = rol;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
