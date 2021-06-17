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
    public partial class Albums : Form
    {
        public Albums()
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
        private void Filter()
        {
            Con.Open();
            string query = "select * from AlbumTable where ACat='" + CatCbSearchCb.SelectedItem.ToString() + "'";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            AlbumDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (ATitleTb.Text == "" || AArtistTb.Text == "" || QtyTb.Text == "" || PriceTb.Text == "" || ACatCb.SelectedIndex == -1)
            {
                MessageBox.Show("Informasi Kurang");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "insert into AlbumTable values ('" + ATitleTb.Text + "', '" + 
                        AArtistTb.Text + "','" + ACatCb.SelectedItem.ToString() + "','" + QtyTb.Text +
                        "', '" + PriceTb.Text + "')";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Album tersimpan");
                    Con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                    Con.Close();
                }
            }
        }

        private void CatCbSearchCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filter();
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            populate();
            CatCbSearchCb.SelectedIndex = -1;
        }
        private void Reset()
        {
            ATitleTb.Text = "";
            AArtistTb.Text = "";
            ACatCb.SelectedIndex = -1;
            PriceTb.Text = "";
            QtyTb.Text = "";
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            Reset();
        }
        int key = 0;
        private void AlbumDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ATitleTb.Text = AlbumDGV.SelectedRows[0].Cells[1].Value.ToString();
            AArtistTb.Text = AlbumDGV.SelectedRows[0].Cells[2].Value.ToString();
            ACatCb.SelectedItem = AlbumDGV.SelectedRows[0].Cells[3].Value.ToString();
            QtyTb.Text = AlbumDGV.SelectedRows[0].Cells[4].Value.ToString();
            PriceTb.Text = AlbumDGV.SelectedRows[0].Cells[5].Value.ToString();

            if (ATitleTb.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(AlbumDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Informasi Kurang");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "delete from AlbumTable where AId=" + key + " ; " ;
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Album terhapus");
                    Con.Close();
                    populate();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (ATitleTb.Text == "" || AArtistTb.Text == ""  || QtyTb.Text == "" || PriceTb.Text == "" || ACatCb.SelectedIndex == -1)
            {
                MessageBox.Show("Informasi Kurang");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "update AlbumTable set ATitle='" + ATitleTb.Text + "', AArtist='" + 
                        AArtistTb.Text+ "', ACat='" + ACatCb.SelectedItem.ToString()+ "', AQty = " + 
                        QtyTb.Text +", APrice = " + PriceTb.Text + " where AId="+ key + ";";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Album tersimpan");
                    Con.Close();
                    populate();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        private void label10_Click(object sender, EventArgs e)
        {
            Login loginn = new Login();
            loginn.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Users user = new Users();
            user.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Dashboard dashh = new Dashboard();
            dashh.Show();
            this.Hide();
        }
    }
}
