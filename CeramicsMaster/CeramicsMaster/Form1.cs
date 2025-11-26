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
    public partial class Form1 : Form
    {
        private SqlConnection connection = new SqlConnection(@"Server=adclg1;Database=Черноусова_22;Integrated Security=True;TrustServerCertificate=true"); //ASS3000\ASS300
        private int try_count = 1;
        public Form1()
        {
            InitializeComponent();
        }

        private void capcha()
        {
            //здесь делаем капчу
            Form2 frm2 = new Form2();
            frm2.ShowDialog();
            try_count++;
        }
        private void taimer()
        {
            Form3 frm3 = new Form3();
            frm3.ShowDialog();
            try_count++;
        }

        private void reload()
        {
            Application.Restart();

        }

        private void wrond_pass()
        {
            switch (try_count)
            {
                case 1:
                    MessageBox.Show("Пароль неверный!", "Неверный пароль", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    capcha();
                    break;
                case 2:
                    MessageBox.Show("Пароль неверный!", "Неверный пароль", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    taimer();
                    break;
                case 3:
                    MessageBox.Show("Пароль неверный, приложение перезапустится!", "Неверный пароль", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    reload();
                    break;
            }
        }

        private void correct_pass()
        {

            Form5 frm5 = new Form5(textBox1.Text);
            this.Hide();
            frm5.ShowDialog();
            this.Show();
            this.Focus();
        }

        private void guest_mode()
        {
            string dtt = DateTime.Now.ToString();
            connection.Open();
            string str_com = $"insert into History values('guest', '{dtt}', 'да')";
            SqlCommand cmd = new SqlCommand(str_com, connection);
            cmd.ExecuteNonQuery();
            connection.Close();

            Form5 frm5 = new Form5("guest");
            this.Hide();
            frm5.ShowDialog();
            this.Show();
            this.Focus();
        }

        private void check_pass()
        {
            string dtt = DateTime.Now.ToString();
            int id = 0;
            string lgn = "";
            string pass = "";
            connection.Open();
            string str_com = $"Select * from users where users.logn = '{textBox1.Text}'  and users.pass = '{textBox2.Text}'";
            SqlCommand cmnd = new SqlCommand(str_com, connection);
            SqlDataReader rdr = cmnd.ExecuteReader();

            while (rdr.Read())
            {
                id = rdr.GetInt32(0);
                lgn = rdr.GetString(1);
                pass = rdr.GetString(2);
            }
            connection.Close();

            if (id == 0)
            {
                connection.Open();
                str_com = $"insert into History values('{textBox1.Text}', '{dtt}', 'нет')";
                SqlCommand cmd = new SqlCommand(str_com, connection);
                cmd.ExecuteNonQuery();
                connection.Close();

                wrond_pass();
            }
            else
            {
                connection.Open();
                str_com = $"insert into History values('{textBox1.Text}', '{dtt}', 'да')";
                SqlCommand cmd = new SqlCommand(str_com, connection);
                cmd.ExecuteNonQuery();
                connection.Close();

                correct_pass();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            check_pass();
        }

        private void textBox2_MouseDown(object sender, MouseEventArgs e)
        {
            textBox2.PasswordChar = '\0';
        }

        private void textBox2_MouseUp(object sender, MouseEventArgs e)
        {
            textBox2.PasswordChar = '*';
        }

        private void button2_Click(object sender, EventArgs e)
        {
            guest_mode();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 frm4 = new Form4();
            frm4.ShowDialog();
        }
    }
}
