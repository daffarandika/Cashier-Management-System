using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;

namespace LKS_Kab_Desktop_2021
{
    
    public partial class Employee : Form
    {
        TextBox[] textBoxes;
        int selectedID;
        public Employee()
        {
            InitializeComponent();
        }
        void getEmpRecord()
        {
            Helper.getRecord(dgvEmp, "select * from employee");
        }
        private void Employee_Load(object sender, EventArgs e)
        {
            textBoxes = new TextBox[] { txtEmail, txtHP, txtName, txtPW };
            getEmpRecord();
            cmbPos.Items.Add("admin");
            cmbPos.Items.Add("cashier");
            cmbPos.Items.Add("chef");
        }
        ErrorProvider ep = new ErrorProvider();
        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (!isTxtValid()){
                return;
            }
            string name, email, password, handphone, position;
            name = txtName.Text;
            email = txtEmail.Text;
            password = Helper.toSha256(txtPW.Text);
            handphone = txtHP.Text;
            position = cmbPos.Text;
            Helper.runQuery("insert into employee (name, email, password, handphone, position) values ('" + name + "', '" + email + "','" + password + "','" + handphone + "','" + position + "')");
            getEmpRecord();
            Helper.clearTxt(textBoxes);
            cmbPos.SelectedIndex = -1;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {

            if (!isTxtValid())
            {
                return;
            }
            string name, email, password, handphone, position;
            name = txtName.Text;
            email = txtEmail.Text;
            password = Helper.toSha256(txtPW.Text);
            handphone = txtHP.Text;
            position = cmbPos.Text;
            Helper.runQuery("update employee set name = '" + name + "', email = '" + email + "', " +
                "password = '" + password + "', handphone = '" + handphone + "', position = '" + position +
                "' where employeeid = '" + selectedID + "'");
            Helper.clearTxt(textBoxes);
            cmbPos.SelectedIndex = -1;
            getEmpRecord();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Helper.runQuery("delete from employee where employeeid = '" + selectedID + "'");
            Helper.clearTxt(textBoxes);
            cmbPos.SelectedIndex = -1;
            getEmpRecord();
        }

        private void dgvEmp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedID = (int)dgvEmp.Rows[e.RowIndex].Cells[0].Value;
            txtName.Text = dgvEmp.Rows[e.RowIndex].Cells["name"].Value.ToString();
            txtEmail.Text = dgvEmp.Rows[e.RowIndex].Cells["email"].Value.ToString();
            txtPW.Text = dgvEmp.Rows[e.RowIndex].Cells["password"].Value.ToString();
            txtHP.Text = dgvEmp.Rows[e.RowIndex].Cells["handphone"].Value.ToString();
            cmbPos.Text = dgvEmp.Rows[e.RowIndex].Cells["position"].Value.ToString();
        }
        bool isTxtValid()
        {
            bool res = true;
            if (Helper.isMt(textBoxes))
            {
                res = false;
            }
            if (!Helper.isEmailValid(txtEmail))
            {
                res = false;
            }
            if (!Helper.isNumber(txtHP))
            {
                res = false;
            }
            //txtName.Text = Helper.isNumber(txtHP).ToString();
            return res;
        }

    }
}
