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

namespace LKS_Kab_Desktop_2021
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            txtPW.UseSystemPasswordChar = true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Helper.isEmailValid(txtEmail))
            {
                ErrorProvider ep = new ErrorProvider();
                ep.SetError(txtEmail, "invalid email address");
                return;
            }
            Helper.conn.Open();
            SqlCommand cmd = new SqlCommand("select * from employee where email = @name and password = @password", Helper.conn);
            cmd.Parameters.AddWithValue("@name", txtEmail.Text);
            cmd.Parameters.AddWithValue("@password", Helper.toSha256(txtPW.Text));
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                Hide();
                Helper.role = reader["position"].ToString();
                Helper.empID = reader["employeeid"].ToString();
                Helper.conn.Close();
                Close();
            }
            else
            {
                MessageBox.Show("Please check your email and password");
            }
            Helper.conn.Close();
        }

        private void showPW_CheckedChanged(object sender, EventArgs e)
        {
            if (showPW.Checked)
            {
                txtPW.UseSystemPasswordChar = false;
                txtPW.Focus();
            } else
            {
                txtPW.UseSystemPasswordChar = true;
                txtPW.Focus();
            }
        }
    }
}
