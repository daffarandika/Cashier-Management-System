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
    public partial class ViewOrder : Form
    {
        public ViewOrder()
        {
            InitializeComponent();
        }

        private void ViewOrder_Load(object sender, EventArgs e)
        {
            getOrderRecord(cmbOrderID.Text.ToString());
            fillComboBoxID();
            fillComboBox();
        }
        void fillComboBox()
        {
            Helper.conn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select * from status", Helper.conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            cmbStatus.ValueMember = "status";
            cmbStatus.DisplayMember = "status";
            cmbStatus.DataSource = dt;
            Helper.conn.Close();
        }
        void getOrderRecord(string menuid)
        {
            Helper.conn.Open();
            SqlCommand cmd = new SqlCommand("select detailid, menu.name, qty, status from detailorder join menu on detailorder.menuid = menu.menuid where orderid = '" + menuid + "'", Helper.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            dgvOrder.Rows.Clear();
            while (reader.Read())
            {
                dgvOrder.Rows.Add(reader["detailid"], reader["name"], reader["qty"], reader["status"]);
            }
            Helper.conn.Close();
            //dgvOrder.Columns["detailid"].Visible = false;
        }
        void fillComboBoxID()
        {
            Helper.conn.Open();
            SqlCommand cmd = new SqlCommand("select distinct orderid from detailorder", Helper.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cmbOrderID.Items.Add(reader["orderid"]);
            }
            Helper.conn.Close();
        }

        private void dgvOrder_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ComboBox combo = (ComboBox)e.Control;

            if (combo != null)
            {
                combo.SelectedIndexChanged -= new EventHandler(cmbStatusChanged);
                combo.SelectedIndexChanged += cmbStatusChanged;
            }

        }
        private void cmbStatusChanged(object sender, EventArgs e)
        {
            string status = ((ComboBox)sender).SelectedValue.ToString();
            int detailid = (int)dgvOrder.CurrentRow.Cells[0].Value;
            Helper.runQuery("update detailorder set status = '" + status + "' where detailid = '" + detailid + "'");
        }

        private void cmbOrderID_SelectedIndexChanged(object sender, EventArgs e)
        {
            getOrderRecord(cmbOrderID.Text);
        }
    }
}
