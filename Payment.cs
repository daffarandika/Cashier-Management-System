using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Kab_Desktop_2021
{
    public partial class Payment : Form
    {
        Object[] nonCash { get; set; }
        Object[] cash { get; set; }
        string paymentMethod;
        string bankUsed;
        public Payment()
        {
            InitializeComponent();
        }

        private void Payment_Load(object sender, EventArgs e)
        {
            nonCash = new Object[]
{
                btnSaveBank, lblBank, lblCardNumber, txtCardNumber, cmbBank
};
            cash = new Object[]
            {
                btnSaveCash, lblBayar, lblKembali, txtBayar, txtKembali
            };
            fillComboBoxID();
            getPaymentRecord(cmbOrderID.Text);
            dgvPayment.Columns.Add("total", "total");
            btnSaveBank.Click += btnSaveBankClick;
            btnSaveCash.Click += btnSaveCashClick;
        }
        private void btnSaveBankClick(object sender, EventArgs e)
        {
            Helper.runQuery("update headorder set bank = '" + cmbBank.Text + "' where orderid = '"+ cmbOrderID.Text +"'");
        }
        private void btnSaveCashClick(object sender, EventArgs e)
        {
            int bayar = Convert.ToInt32(txtBayar.Text);
            txtKembali.Text = Convert.ToString(bayar - calculateTotal());
        }

        private void getPaymentRecord(string orderid)
        {
            Helper.getRecord(dgvPayment, "select menu.name, qty, detailorder.price from detailorder join menu on detailorder.menuid = menu.menuid where orderid = '" + orderid + "'");
            foreach (DataGridViewRow r in dgvPayment.Rows)
            {
                // dgvPayment.Columns.Add(new DataGridViewColumn ());
                r.Cells["total"].Value = Convert.ToString(Convert.ToInt32(r.Cells["price"].Value) * Convert.ToInt32(r.Cells["qty"].Value));
            }
        }

        private void cmbOrderID_SelectedIndexChanged(object sender, EventArgs e)
        {
            setComponents();
            Helper.conn.Open();
            SqlCommand cmd = new SqlCommand("select * from headorder where orderid = '" + cmbOrderID.Text + "'", Helper.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            paymentMethod = reader["payment"].ToString();
            bankUsed = reader["bank"].ToString();
            Helper.conn.Close();
            cmbPayment.Text = paymentMethod;
            getPaymentRecord(cmbOrderID.Text);
            lblRes.Text = calculateTotal().ToString("N0");
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
        void fillCmbBank()
        {
            cmbBank.Items.Clear();
            cmbBank.Items.Add("BNI");
            cmbBank.Items.Add("BRI");
            cmbBank.Items.Add("BCA");
            cmbBank.Items.Add("BTN");
        }
        void setComponents()
        {
            //string paymentMethod = "0";
            if (paymentMethod == "Tunai")
            {
                cashPayment();
            }
            else if (paymentMethod == "Debit" || paymentMethod == "Credit")
            {
                bankPayment();
            }
            else
            {
                paymentNotSet();
            }
        }
        void bankPayment()
        {
            foreach (var item in cash)
            {
                Controls.Remove((Control)item);
            }
            foreach (var item in nonCash)
            {
                Controls.Add((Control)item);
            }
            fillCmbBank();
            cmbBank.Text = bankUsed;
            cmbPayment.Text = paymentMethod;
        }
        void cashPayment()
        {
            foreach (var item in nonCash)
            {
                Controls.Remove((Control)item);
            }
            foreach (var item in cash)
            {
                Controls.Add((Control)item);
            }
        }
        void paymentNotSet()
        {

        }
        int calculateTotal()
        {
            int sum = 0;
            foreach (DataGridViewRow r in dgvPayment.Rows)
            {
                sum += Convert.ToInt32(r.Cells["total"].Value);
            }
            return sum;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Helper.runQuery("update headorder set payment = '" + cmbPayment.Text + "' where orderid = '" + cmbOrderID.Text + "'");
            paymentMethod = cmbPayment.Text;
            setComponents();
        }
        //dumping bank related components

        Button btnSaveBank = new Button()
        {
            Text = "Save",
            Location = new Point(146, 452),
        };
        Label lblBank = new Label()
        {
            Text = "Bank",
            Location = new Point(26, 393),
            AutoSize = true
        };
        Label lblCardNumber = new Label()
        {
            Text = "Card Number",
            Location = new Point(26, 423)
        };
        ComboBox cmbBank = new ComboBox()
        {
            Text = "--select bank--",
            Location = new Point(146, 390),
            Size = new Size(267, 23)
        };
        TextBox txtCardNumber = new TextBox()
        {
            Size = new Size(267, 23),
            Location = new Point(146, 419)
        };
        // dumping cash related components
        Label lblBayar = new Label()
        {
            Text = "Bayar",
            Location = new Point(26, 393),
            AutoSize = true
        };
        Label lblKembali = new Label()
        {
            Text = "Kembali",
            Location = new Point(26, 423)
        };


        TextBox txtBayar = new TextBox()
        {
            Location = new Point(146, 390),
            Size = new Size(267, 23)
        };

        TextBox txtKembali = new TextBox()
        {
            Size = new Size(267, 23),
            Location = new Point(146, 419)
        };
        Button btnSaveCash = new Button()
        {
            Text = "Save",
            Location = new Point(146, 452),
        };
    }
}
