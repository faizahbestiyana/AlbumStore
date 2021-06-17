using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AlbumStore
{
    public partial class Billing : Form
    {
        public Billing()
        {
            InitializeComponent();
            populate();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Acer\source\repos\AlbumStore\AlbumDb.mdf;Integrated Security=True");
        private void populate()
        {
            Con.Open();
            string query = "select * from AlbumTable";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            AlbumDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void UpdateAlbum()
        {
            int newQty = stock - Convert.ToInt32(QtyTb.Text);
            try
            {
                Con.Open();
                string query = "update AlbumTable set AQty = " + newQty + " where AId=" + key + ";";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                Con.Close();
                populate();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
        int n = 0, GrandTotal = 0;
        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (QtyTb.Text == "" || Convert.ToInt32(QtyTb.Text) > stock)
            {
                MessageBox.Show("Stock Tidak Cukup");
            }
            else
            {
                int total = Convert.ToInt32(QtyTb.Text) * Convert.ToInt32(PriceTb.Text);
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(BillDGV);
                newRow.Cells[0].Value = n + 1;
                newRow.Cells[1].Value = ATitleTb.Text;
                newRow.Cells[2].Value = PriceTb.Text;
                newRow.Cells[3].Value = QtyTb.Text;
                newRow.Cells[4].Value = total;
                BillDGV.Rows.Add(newRow);
                n++;
                UpdateAlbum();
                GrandTotal += total;
                totalLbl.Text = "Rp " + GrandTotal;
            }
        }
        int key = 0, stock = 0;
        private void AlbumDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ATitleTb.Text = AlbumDGV.SelectedRows[0].Cells[1].Value.ToString();
            PriceTb.Text = AlbumDGV.SelectedRows[0].Cells[5].Value.ToString();

            if (ATitleTb.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(AlbumDGV.SelectedRows[0].Cells[0].Value.ToString());
                stock = Convert.ToInt32(AlbumDGV.SelectedRows[0].Cells[4].Value.ToString());
            }
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            if (UserNameLbl.Text == "" || ATitleTb.Text == ""|| ClientNameTb.Text == "")
            {
                MessageBox.Show("Ketik Nama Client");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "insert into BillTable values ('" + UserNameLbl.Text + "', '" +
                        ClientNameTb.Text + "','" + GrandTotal + "')";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Tagihan tersimpan");
                    Con.Close();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                    Con.Close();
                }
                printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprm", 500, 600);
                    if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                    {
                        printDocument1.Print();
                    }
            }
        }
        int productid, productqty, productprice, totall, pos = 60;

        private void Billing_Load(object sender, EventArgs e)
             {
                UserNameLbl.Text = Login.UserName;
             }

            string productname;
            private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
            {
                e.Graphics.DrawString("Noona's Store", new Font("Georgia", 12, FontStyle.Bold), Brushes.DarkOrange, new Point(100));
                e.Graphics.DrawString("Harga Produk Total :", new Font("Georgia", 8, FontStyle.Bold), Brushes.DarkOrange, new Point(100, 40));
                foreach (DataGridViewRow row in BillDGV.Rows)
                {
                    productid = Convert.ToInt32(row.Cells["Column1"].Value);
                    productname = "" + row.Cells["Column2"].Value;
                    productprice = Convert.ToInt32(row.Cells["Column3"].Value);
                    productqty = Convert.ToInt32(row.Cells["Column4"].Value);
                    totall = Convert.ToInt32(row.Cells["Column5"].Value);
                    e.Graphics.DrawString("" + productid, new Font("Georgia", 8, FontStyle.Regular), Brushes.SandyBrown, new Point(26, pos));
                    e.Graphics.DrawString("" + productname, new Font("Georgia", 8, FontStyle.Regular), Brushes.SandyBrown, new Point(45, pos));
                    e.Graphics.DrawString("" + productprice, new Font("Georgia", 8, FontStyle.Regular), Brushes.SandyBrown, new Point(200, pos));
                    e.Graphics.DrawString("" + productqty, new Font("Georgia", 8, FontStyle.Regular), Brushes.SandyBrown, new Point(240, pos));
                    e.Graphics.DrawString("" + totall, new Font("Georgia", 8, FontStyle.Regular), Brushes.SandyBrown, new Point(275, pos));
                    pos += 20;
                }
                e.Graphics.DrawString("Grand Total: Rp " + GrandTotal + " ribu", new Font("Georgia", 10, FontStyle.Bold), Brushes.Salmon, new Point(150, pos + 50));
                e.Graphics.DrawString("Terimakasih sudah berbelanja, bahagia selalu^^", new Font("Georgia", 8, FontStyle.Bold), Brushes.Salmon, new Point(100, pos + 85));
                e.Graphics.DrawString("-----------Noona's Store-----------", new Font("Georgia", 8, FontStyle.Bold), Brushes.Salmon, new Point(170, pos + 120));
                BillDGV.Rows.Clear();
                BillDGV.Refresh();
                pos = 100;
                GrandTotal = 0;
            }
            private void Reset()
            {
                ATitleTb.Text = "";
                PriceTb.Text = "";
                QtyTb.Text = "";
                ClientNameTb.Text = "";
            }
            private void ResetBtn_Click(object sender, EventArgs e)
            {
                Reset();
            }
        private void LogoutLbl_Click(object sender, EventArgs e)
        {
            Login loginn = new Login();
            loginn.Show();
            this.Hide();
        }
    }
}
