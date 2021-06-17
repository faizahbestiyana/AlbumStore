using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AlbumStore
{
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            if (UPassTb.Text == "iniadmin")
            {
                Albums albums = new Albums();
                albums.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("password salah, silahkan kontak developer");
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }
    }
 }
