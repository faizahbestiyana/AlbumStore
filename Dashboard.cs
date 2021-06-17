using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AlbumStore
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            Login loginn = new Login();
            loginn.Show();
            this.Hide();
        }
        private void label7_Click(object sender, EventArgs e)
        {
            Albums albums = new Albums();
            albums.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Users user = new Users();
            user.Show();
            this.Hide();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Acer\source\repos\AlbumStore\AlbumDb.mdf;Integrated Security=True");

        private void Dashboard_Load(object sender, EventArgs e)
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select sum(AQty) from AlbumTable", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            AlbumStockLbl.Text = dt.Rows[0][0].ToString();
            SqlDataAdapter sda1 = new SqlDataAdapter("select sum(Amount) from BillTable", Con);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            AmountStockLbl.Text = dt1.Rows[0][0].ToString();
            SqlDataAdapter sda2 = new SqlDataAdapter("select Count(*) from UserTable", Con);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            UserStockLbl.Text = dt2.Rows[0][0].ToString();
            Con.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
