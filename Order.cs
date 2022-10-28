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
    public partial class Order : Form
    {
        TextBox[] textBoxes;
        string empID;
        string menuid_value;
        public Order(string empID)
        {
            this.empID = empID;
            InitializeComponent();
        }

        private void Order_Load(object sender, EventArgs e)
        {
            textBoxes = new TextBox[]
            {
                txtName, txtPrice, txtQty
            };
            Helper.getRecord(dgvMenu, "select menuid, name, price, photo from menu");
            dgvMenu.Columns["menuid"].Visible = false;
            dgvMenu.Columns["photo"].Visible = false;
            fillMemberCombo();
        }
        private void fillMemberCombo()
        {
            Helper.conn.Open();
            SqlCommand cmd = new SqlCommand("select * from member order by name", Helper.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("memberid");
            dt.Columns.Add("name");

            while (reader.Read())
            {
                dt.Rows.Add(reader["memberid"], reader["name"]);
            }

            Helper.conn.Close();

            cmbMember.DisplayMember = "name";
            cmbMember.ValueMember = "memberid";
            cmbMember.DataSource = dt;
            cmbMember.Text = "--select member--";
        }

        private void dgvMenu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = dgvMenu.CurrentRow.Cells["name"].Value.ToString();
            txtPrice.Text = dgvMenu.CurrentRow.Cells["price"].Value.ToString();
            pbOrder.Image = Image.FromFile(@"C:\Users\ASUS\source\repos\LKS_Kab_Desktop_2021\Images\" + dgvMenu.CurrentRow.Cells["photo"].Value.ToString());
            menuid_value = dgvMenu.CurrentRow.Cells["menuid"].Value.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //if (!isInputValid())
            //{
            //    return;
            //}
            //isInputValid();
            int total = Convert.ToInt32(txtPrice.Text) * Convert.ToInt32(txtQty.Text);
            dgvOrder.Rows.Add(menuid_value, txtName.Text, txtQty.Text, txtPrice.Text, total.ToString());

            int res = 0;
            foreach (DataGridViewRow r in dgvOrder.Rows)
            {
                res += Convert.ToInt32(r.Cells["total"].Value);
            }
            lblRes.Text = res.ToString();
            
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            //for headorder
            string orderid = Helper.generateOrderID();
            //string orderid = "2022090005";
            string employeeid = empID;
            string memberid = cmbMember.SelectedValue.ToString();

            Helper.runQuery("insert into headorder values ('" + orderid + "', '" + employeeid + "', '" + memberid + "', cast (GETDATE() as DATE), 'Tunai', '')");
            foreach (DataGridViewRow r in dgvOrder.Rows)
            {
                //for detailorder
                string qty = r.Cells["qty"].Value.ToString();
                string price = r.Cells["price"].Value.ToString();
                string menuid = r.Cells["menuid"].Value.ToString();
                Helper.runQuery("insert into detailorder (orderid, menuid, qty, price, status) values ('" + orderid + "', '" + menuid + "', '" + qty + "'" +
                    ", '" + price + "', 'Cooking')");
            }
            // update income
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.ToString("yyyy");
            Helper.conn.Open();
            SqlCommand cmd = new SqlCommand("select * from income where month = '" + month + "' and year = '" + year + "'", Helper.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            string income = "";
            while (reader.Read())
            {
                income = reader["income"].ToString();
            }
            Helper.conn.Close();
            Helper.runQuery("update income set income = '" + (Convert.ToInt32(income) + calculateTotal()) + "' where month = '" + month + "' and year = '" + year + "'");
        }
        int calculateTotal()
        {
            int res = 0;
            foreach (DataGridViewRow r in dgvOrder.Rows)
            {
                res += Convert.ToInt32(r.Cells["total"].Value);
            }
            return res;
        }
        bool isInputValid()
        {
            bool res = true;
            
            if (Helper.isNumber(txtQty))
            {
                res = false;
            }
            return res;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Helper.clearTxt(textBoxes);
            cmbMember.Text = "--select member--";
            dgvOrder.Rows.Clear();
        }
    }
}
      