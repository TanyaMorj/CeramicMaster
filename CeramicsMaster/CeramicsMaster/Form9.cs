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
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CeramicsMaster
{
    public partial class Form9: Form
    {
        private SqlConnection connection = new SqlConnection(@"Server=adclg1;Database=Черноусова_22;Integrated Security=True;TrustServerCertificate=true");
        
        public Form9()
        {
            InitializeComponent();
            fill_the_list();
            fill_the_panel();
        }

        private void fill_the_list()
        {
            comboBox1.Items.Add("Все продукты");
            connection.Open();
            string str_com = "select Product_name from Products_import";
            SqlCommand cmnd = new SqlCommand(str_com, connection);
            SqlDataReader rdr = cmnd.ExecuteReader();

            while (rdr.Read())
            {
                comboBox1.Items.Add(rdr.GetString(0));
            }
            connection.Close();

        }

        private void fill_the_panel(string usl="is not null")
        {

            panel1.Controls.Clear();

            connection.Open();
            string str_com = $"select * from Product_materials_import where Products {usl}";
            SqlCommand cmnd = new SqlCommand(str_com, connection);
            SqlDataReader rdr = cmnd.ExecuteReader();
            int i = 0;

            while (rdr.Read())
            {
                var pnl = new Panel {
                    BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                    Location = new System.Drawing.Point(6, 4 + 150 * i),
                    Size = new System.Drawing.Size(730, 130),
                    TabIndex = 0
                };

                var prd = new Label 
                {
                    AutoSize = true,
                    Font = new System.Drawing.Font("Gabriola", 15.75F),
                    Location = new System.Drawing.Point(4, 4),
                    Size = new System.Drawing.Size(78, 39),
                    Name = "Product" + i.ToString(),
                    TabIndex = 0,
                    Text = rdr.GetString(0)
                };

                var mat = new Label
                {
                    AutoSize = true,
                    Cursor = System.Windows.Forms.Cursors.Hand,
                    BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(217)))), ((int)(((byte)(178))))),
                    Font = new System.Drawing.Font("Gabriola", 15.75F),
                    Location = new System.Drawing.Point(11, 47),
                    Size = new System.Drawing.Size(90, 39),
                    TabIndex = 1,
                    Text = rdr.GetString(1)
                };
                mat.Click += Prd_Click;


                var kol = new Label
                {
                    AutoSize = true,
                    Font = new System.Drawing.Font("Gabriola", 15.75F),
                    Location = new System.Drawing.Point(378, 89),
                    Size = new System.Drawing.Size(265, 39),
                    TabIndex = 2,
                    Text = "Необходимо для единицы продукта:"
                };

                var kol_zn = new Label
                {
                    AutoSize = true,
                    Font = new System.Drawing.Font("Gabriola", 15.75F),
                    Location = new System.Drawing.Point(649, 89),
                    Size = new System.Drawing.Size(46, 39),
                    TabIndex = 3,
                    Text = Math.Round(rdr.GetDecimal(2), 2).ToString()
                };

                pnl.Controls.Add(prd);
                pnl.Controls.Add(mat);
                pnl.Controls.Add(kol);
                pnl.Controls.Add(kol_zn);

                panel1.Controls.Add(pnl);
                i++;
            }
            connection.Close();
     

        }

        private void Prd_Click(object sender, EventArgs e)
        {
            Label lb = sender as Label;
            label11.Text = lb.Text;

            Panel pnl = lb.Parent as Panel;
            var allLab = pnl.Controls.OfType<Label>();

            foreach (Label lbb in allLab)
            {
                if (lbb.Name.Contains("Product"))
                {
                    label7.Text = lbb.Text;
                }
            }
        }

        private int Calculate_amount_materials(string prod, string matr, int kol, double koeff, double perc, int kol_mat)
        {
            if (kol > 0 && koeff > 0 && perc > 0 && kol_mat > 0)
            {
                int ress = Convert.ToInt32(perc * koeff * kol + (perc * koeff * kol) * perc);
                
                return ress;
            }
            return -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string prod = "";
            string matr = "";
            int kol = Convert.ToInt32(textBox1.Text);
            double koeff = 0;
            double perc = 0;
            int kol_mat = 0;

            connection.Open();
            string str_com = "select Products_import.Product_type, " +
                "Materials_import.Material_type, " +
                "Materials_import.Quantity_in_stock, " +
                "Material_type_import.Percent_of_defects, " +
                "Product_type_import.Product_type_coefficient " +
                "from Product_type_import inner join " +
                "(Products_import inner join " +
                "(Product_materials_import inner join " +
                "(Materials_import inner join Material_type_import " +
                "on Materials_import.Material_type = Material_type_import.Material_type) " +
                "on Product_materials_import.Material_name = Materials_import.Material_name) " +
                "on Products_import.Product_name = Product_materials_import.Products) " +
                "on Product_type_import.Type_of_products = Products_import.Product_type " +
                $"where Products_import.Product_name = '{label7.Text}' and Materials_import.Material_name = '{label11.Text}';";
            SqlCommand cmnd = new SqlCommand(str_com, connection);
            SqlDataReader rdr = cmnd.ExecuteReader();

            while (rdr.Read())
            {
                prod = rdr.GetString(0);
                matr = rdr.GetString(1);
                kol_mat = rdr.GetInt16(2);
                perc = Convert.ToDouble((rdr.GetString(3).Substring(0, rdr.GetString(3).Length - 1)).Replace('.', ','));
                koeff = Convert.ToDouble(rdr.GetDecimal(4));
            }

            int res = Calculate_amount_materials(prod, matr, kol, koeff, perc, kol_mat);

            textBox2.Text = res.ToString();

            int rss = res - kol_mat;
            if (rss < 0)
                rss = 0;

            textBox3.Text = rss.ToString();

            connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            string usl = "";
            if (comboBox1.Text != "" && comboBox1.Text != "Все продукты")
            {
                usl = $" = '{comboBox1.Text}'";
                label7.Text = comboBox1.Text;
            }
            else
            {
                usl = $"is not null";
                label7.Text = "Ничего не выбрано";
            }
            fill_the_panel(usl);
        }
    }
}
