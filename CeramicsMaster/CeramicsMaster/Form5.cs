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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CeramicsMaster
{
    public partial class Form5 : Form
    {
        private SqlConnection connection = new SqlConnection(@"Server=adclg1;Database=Черноусова_22;Integrated Security=True;TrustServerCertificate=true");
        private string lognn = "";
        public Form5(string logn)
        {
            this.lognn = logn;
            
            InitializeComponent();

            open_user_info();
            fill_the_list();
            add_component();
            visible_of_component();
        }

        private void visible_of_component()
        {
            switch (lognn)
            {
                case "guest":
                    panel3.Visible = false;
                    panel4.Visible = false;
                    panel5.Visible = false;
                    panel6.Visible = false;
                    panel7.Visible = false;
                    break;
                case "manag":
                    panel6.Visible = false;
                    break;
                case "klient":
                    panel7.Visible = false;
                    panel6.Visible = false;
                    break;
                case "admin":
                    panel7.Visible = false;
                    break;
            }
        }

        private void fill_the_list()
        {
            comboBox1.Items.Add("Все типы");
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

        private void add_component(string typ="is not null", string nam="is not null", string sort="")
        {
            //здесь добавляются компоненты

            panel1.Controls.Clear();

            string product_type = "";
            string product_name = "";
            int article = 0;
            decimal min_price_for_partner = 0;
            decimal roll_width_m = 0;
            decimal full_price = 0;
            int i = 0;

            connection.Open();
            string str_com = "Select Products_import.Product_type, " +
                "Products_import.Product_name, " +
                "Products_import.Article, " +
                "Products_import.Minimum_price_for_partner, " +
                "Products_import.Roll_width_m, " +
                "SUM(Materials_import.Price_unit_material * Product_materials_import.Required_amount_of_material)  as full_price " +
                "from Materials_import inner join (Product_materials_import inner join Products_import on (Product_materials_import.Products = Products_import.Product_name))" +
                " on (Materials_import.Material_name = Product_materials_import.Material_name) " +
                $"Where Products_import.Product_type {typ} and Products_import.Product_name {nam} " +
                "GROUP BY" +
                "    Products_import.Product_type," +
                "    Products_import.Product_name," +
                "    Products_import.Article," +
                "    Products_import.Minimum_price_for_partner," +
                "    Products_import.Roll_width_m " + sort;
            SqlCommand cmnd = new SqlCommand(str_com, connection);
            SqlDataReader rdr = cmnd.ExecuteReader();
            

            while (rdr.Read())
            {
                product_type = rdr.GetString(0);
                product_name = rdr.GetString(1);
                article = rdr.GetInt32(2);
                min_price_for_partner = Math.Round(rdr.GetDecimal(3), 2);
                roll_width_m = Math.Round(rdr.GetDecimal(4), 2);
                full_price = Math.Round(rdr.GetDecimal(5), 2);

                var pnl = new Panel
                {
                    AutoScroll = true,
                    Location = new System.Drawing.Point(4, 4 + 180 * i),
                    Size = new System.Drawing.Size(840, 170),
                    TabIndex = 0,
                    BorderStyle = BorderStyle.FixedSingle
                };
                var tip = new Label
                {
                    AutoSize = true,
                    Font = new System.Drawing.Font("Gabriola", 15.75F),
                    Location = new System.Drawing.Point(10, 4),
                    Size = new System.Drawing.Size(42, 39),
                    TabIndex = 0,
                    Text = product_type
                };

                var nm = new Label
                {
                    AutoSize = true,
                    BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(217)))), ((int)(((byte)(178))))),
                    Cursor = System.Windows.Forms.Cursors.Hand,
                    Font = new System.Drawing.Font("Gabriola", 15.75F),
                    Location = new System.Drawing.Point(262, 4),
                    Size = new System.Drawing.Size(188, 39),
                    TabIndex = 1,
                    Text = product_name,
                };

                nm.Click += Nm_Click;

                var art = new Label
                {
                    AutoSize = true,
                    Font = new System.Drawing.Font("Gabriola", 14.25F),
                    Location = new System.Drawing.Point(11, 47),
                    Size = new System.Drawing.Size(70, 35),
                    TabIndex = 2,
                    Text = "Артикул"
                };

                var art_znch = new Label
                {
                    AutoSize = true,
                    Font = new System.Drawing.Font("Gabriola", 14.25F),
                    Location = new System.Drawing.Point(102, 47),
                    Size = new System.Drawing.Size(63, 35),
                    TabIndex = 4,
                    Text = article.ToString()
                };

                var min_st = new Label
                {
                    AutoSize = true,
                    Font = new System.Drawing.Font("Gabriola", 14.25F),
                    Location = new System.Drawing.Point(11, 82),
                    Size = new System.Drawing.Size(281, 35),
                    TabIndex = 3,
                    Text = "Минимальная стоимость для партнёра(р)"
                };

                var min_st_znch = new Label
                {
                    AutoSize = true,
                    Font = new System.Drawing.Font("Gabriola", 14.25F),
                    Location = new System.Drawing.Point(298, 82),
                    Size = new System.Drawing.Size(54, 35),
                    TabIndex = 5,
                    Text = min_price_for_partner.ToString()
                };

                var shir = new Label
                {
                    AutoSize = true,
                    Font = new System.Drawing.Font("Gabriola", 14.25F),
                    Location = new System.Drawing.Point(11, 117),
                    Size = new System.Drawing.Size(90, 35),
                    TabIndex = 6,
                    Text = "Ширина (м)"
                };

                var shir_znch = new Label
                {
                    AutoSize = true,
                    Font = new System.Drawing.Font("Gabriola", 14.25F),
                    Location = new System.Drawing.Point(107, 117),
                    Size = new System.Drawing.Size(55, 35),
                    TabIndex = 7,
                    Text = roll_width_m.ToString()
                };

                var price = new Label
                {
                    AutoSize = true,
                    Font = new System.Drawing.Font("Gabriola", 15.75F),
                    Location = new System.Drawing.Point(523, 45),
                    Size = new System.Drawing.Size(120, 39),
                    TabIndex = 8,
                    Text = "Стоимость (р)"
                };

                var price_znch = new Label
                {
                    AutoSize = true,
                    Font = new System.Drawing.Font("Gabriola", 14.25F),
                    Location = new System.Drawing.Point(662, 47),
                    Name = "label10",
                    Size = new System.Drawing.Size(54, 35),
                    TabIndex = 9,
                    Text = full_price.ToString()
                };

                pnl.Controls.Add(tip);
                pnl.Controls.Add(nm);
                pnl.Controls.Add(art);
                pnl.Controls.Add(art_znch);
                pnl.Controls.Add(min_st);
                pnl.Controls.Add(min_st_znch);
                pnl.Controls.Add(shir);
                pnl.Controls.Add(shir_znch);
                pnl.Controls.Add(price);
                pnl.Controls.Add(price_znch);


                panel1.Controls.Add(pnl);
                i++;

            }
            connection.Close();

        }

        private void Nm_Click(object sender, EventArgs e)
        {
            if (lognn == "admin")
            {
                Label lb = sender as Label;
                Form7 frm7 = new Form7(lb.Text);
                frm7.ShowDialog();
                add_component();
            }
            
        }

        private void open_user_info()
        {
            Form6 frm6 = new Form6(lognn);
            frm6.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            open_user_info();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Наименование")
            {
                textBox2.Text = "";
                textBox2.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Наименование";
                textBox2.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sort = "ORDER BY full_price DESC";
            add_component(sort: sort);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string sort = "ORDER BY full_price ASC";
            add_component(sort: sort);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string typ = $" = '{comboBox1.Text}'";

            if (comboBox1.Text == "Все типы")
            {
                typ = "is not null";
            }

            add_component(typ);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form8 frm8 = new Form8();
            frm8.ShowDialog();

            add_component();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form9 frm9 = new Form9();
            frm9.ShowDialog();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            string nam = $" Like '{textBox2.Text}%'";

            if (textBox2.Text == "Наименование")
            {
                nam = "is not null";
            }

            add_component(nam: nam);
        }
    }
}
