create table Unit_measurement
(
id int not null primary key,
unit_name nvarchar(50)
)

create table Producers
(
id int not null primary key,
producer_name nvarchar(50)
)

create table Providers
(
id int not null primary key,
provider_name nvarchar(50)
)

create table Categories
(
id int not null primary key,
category_name nvarchar(50)
)

create table Roles
(
id int not null primary key,
role_name nvarchar(50)
)

create table Users
(
id int not null primary key,
id_role int not null foreign key references Roles(id),
second_name nvarchar(50),
name_user nvarchar(50),
middle_name nvarchar(50),
user_login nvarchar(50),
user_password nvarchar(50)
)

create table Point_address
(
id int not null primary key,
postmat_id nvarchar(50),
city_name nvarchar(50),
street_name nvarchar(50)
)

create table Products
(
id int not null identity(1,1) primary key,
articl nvarchar(50),
product_name nvarchar(200),
id_unit_measurement int not null foreign key references Unit_measurement(id),
price decimal(10,2),
id_provider int not null foreign key references Providers(id),
id_producer int not null foreign key references Producers(id),
id_category int not null foreign key references Categories(id),
discount int,
count_on_store int,
inform nvarchar(300),
photo nvarchar(50)
)

create table Order_statuses
(
id int not null primary key,
order_status nvarchar(50)
)

create table Orders
(
id int not null identity(1,1) primary key,
order_date date,
delivery_date date,
id_point int not null foreign key references Point_address(id),
id_user int not null foreign key references Users(id),
code nvarchar(50),
id_status int not null foreign key references Order_statuses(id)
)

create table orders_and_products
(
id_order int not null foreign key references Orders(id),
id_product int not null foreign key references Products(id),
product_count int
)
 
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
 
internal class BaseCon
{
    SqlConnection basecon = new SqlConnection("Server=ASS3000\\ASS300;Database=Toy_store;Trusted_Connection=True;"); //;User Id=user04;Password=93499;"); // ;Trusted_Connection=True

    public void open_connection()
    {
        basecon.Open();
    }

    public void close_connection() { basecon.Close(); }
    public SqlConnection get_connection() { return basecon; }
}
 
 
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
 
private void fill_the_panel(string new_usl="")
{
    panel2.Controls.Clear();
    SqlCommand cmd = new SqlCommand("select [Products].[id],[art],[product_name],Unit_measuremnt.[unit_name],[price],Deliver.[deliver_name]      ,Creator.creator_name      ,Categories.category_name      ,[discont]      ,[countt]      ,[info]      ,[photo] from ((([Products] inner join Unit_measuremnt on Unit_measuremnt.id = [Products].unit_meas) inner join Deliver on Products.deliver = Deliver.id)      inner join Creator on Creator.id = Products.creator) inner join Categories on Categories.id = Products.category" + new_usl, conn);

    SqlDataReader rdr = cmd.ExecuteReader();
    int i = 0;

    while (rdr.Read())
    {
        string new_price = "";
        string price = rdr.GetValue(4).ToString();
        string picka = "picture.png";
        Color color_for_dis = System.Drawing.Color.BurlyWood;
        Color color_for_price = System.Drawing.Color.White;
        Font font_for_price = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        Panel panel = new Panel();
        panel.Tag = rdr.GetInt32(0);
        panel.Location = new System.Drawing.Point(4, 4 + i*250);
        panel.Size = new System.Drawing.Size(1066, 221);
        picka = rdr.GetString(11);
        if (string.IsNullOrEmpty(picka))
        {
            picka = "picture.png";
        }

        int discont = Convert.ToInt32(rdr.GetValue(8));
        if (discont > 0)
        {
            new_price = (Convert.ToInt32(price) - Convert.ToInt32(price) * 0.01 * discont).ToString();
            color_for_price = System.Drawing.Color.Red;
            font_for_price = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            if (discont > 17)
                color_for_dis = System.Drawing.Color.NavajoWhite;

        }

        panel.Controls.AddRange(new Control[]
        { new Label{AutoSize = true,
        Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204))), Location = new System.Drawing.Point(199, 4),
        Text = $"{rdr.GetString(7)} | {rdr.GetString(2)}"},

        new Label{AutoSize = true,
        Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204))), Location = new System.Drawing.Point(199, 26),
        Text = $"Описание: {rdr.GetString(10)}"},

        new Label{AutoSize = true,
        Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204))), Location = new System.Drawing.Point(199, 51),
        Text = $"Производитель: {rdr.GetString(6)}"},

        new Label{AutoSize = true,
        Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204))), Location = new System.Drawing.Point(199, 78),
        Text = $"Поставщик: {rdr.GetString(5)}"},

        new Label{AutoSize = true,
        Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204))), Location = new System.Drawing.Point(199, 104),
        Text = $"Цена:"},

        new Label{AutoSize = true,
        Font = font_for_price, Location = new System.Drawing.Point(294, 104),
        Text = $"{price}", BackColor = color_for_dis},

        new Label{AutoSize = true,
        Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204))), Location = new System.Drawing.Point(336, 104),
        Text = $"{new_price}"},

        new Label{AutoSize = true,
        Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204))), Location = new System.Drawing.Point(198, 126),
        Text = $"Единица измерения: {rdr.GetValue(3)}"},

        new Label{AutoSize = true,
        Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204))), Location = new System.Drawing.Point(198, 154),
        Text = $"Кол-во на складе: {rdr.GetValue(9)}"},

        new Label{Size = new System.Drawing.Size(222, 148),
        Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204))), Location = new System.Drawing.Point(807, 59), TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
        Text = $"{rdr.GetValue(8)} %", BackColor = color_for_dis},

        new PictureBox{
            Location = new System.Drawing.Point(4, 4),
    Size = new System.Drawing.Size(188, 149), Image = Image.FromFile(picka), SizeMode = PictureBoxSizeMode.Zoom
        }


        });
        if (is_admin)
        panel.Click += Panel_Click;

        panel2.Controls.Add(panel);
        i++;
    }
    rdr.Close();
}
 
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
 
private void set_new_pattern()
{
    string name = string.IsNullOrEmpty(textBox4.Text) ? "%" : textBox4.Text;
    string sup = comboBox3.Text == "Все поставщики" ? "%" : comboBox3.Text;

    string cou = comboBox1.Text == "Возрастание" ? "ASC" : "Desc";
    string pri = comboBox2.Text == "Возрастание" ? "ASC" : "Desc";
    string pattent = $" where (product_name like N'%{name}%' or Unit_measuremnt.[unit_name] like N'%{name}%' " +
        $"or Creator.creator_name like N'%{name}%' or Categories.category_name like N'%{name}%'  or [info] like N'%{name}%') and Deliver.[deliver_name] like N'%{sup}%' " +
        $"order by price {pri}, countt {cou}";
    fill_the_panel(pattent);
}
 
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
 
private bool check_the_spaces()
{
    IEnumerable<System.Windows.Forms.TextBox> textboxes = this.Controls.OfType<System.Windows.Forms.TextBox>();
    foreach (System.Windows.Forms.TextBox textBox in textboxes)
    {
        if (string.IsNullOrEmpty(textBox.Text))
        {
            MessageBox.Show("Не все поля заполнены", "Провал!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }
    }
    try
    {
        int pr = Convert.ToInt32(textBox4.Text);
        int cou = Convert.ToInt32(textBox8.Text);
    }
    catch 
    {
        MessageBox.Show("Неверный формат данных", "Провал!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return false;
    }
    return true;
}
 
 
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
 
private void button3_Click(object sender, EventArgs e)
{
    using (OpenFileDialog ofd = new OpenFileDialog())
    {
        ofd.Filter = "Изображения|*.jpg;*.jpeg;*.png";
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            using (var img = Image.FromFile(ofd.FileName))
            {
                if (img.Height > 320 || img.Width > 320)
                {
                    MessageBox.Show("Изображение слишком большое, выберите изображение меньше 300 пикселей", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            pictureBox1.Image = Image.FromFile(ofd.FileName);
            curr_img = Path.GetFileName(ofd.FileName);
            way_to_img = ofd.FileName;
        }
    }
}


^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

private void change_the_img()
{
    string filnam = Path.GetFileName(way_to_img);
    string way_to_copy = Path.Combine(Application.StartupPath, filnam);

    try
    {
        File.Copy(way_to_img, way_to_copy);
    }
    catch { }

}