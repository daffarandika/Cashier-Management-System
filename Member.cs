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
    public partial class Member : Form
    {
        TextBox[] textBoxes;
        int selectedIndex;
        public Member()
        {
            InitializeComponent();
        }

        private void Member_Load(object sender, EventArgs e)
        {
            textBoxes = new TextBox[]
            {
                txtEmail, txtHP, txtName
            };
            getMemberRecord();
        }

        private void getMemberRecord()
        {
            Helper.getRecord(dgvMember, "select * from member");
            string dateInTable = Helper.searchDateInTable("member");
            if (dateInTable != "")
            {
                dgvMember.Columns[dateInTable].DefaultCellStyle.Format = "dd-MM-yyyy";
            }
        }
        void clearMember()
        {
            Helper.clearTxt(textBoxes);
        }
        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (!isInputValid())
            {
                return;
            }
            string name, email, handphone;
            name = txtName.Text;
            email = txtEmail.Text;
            handphone = txtHP.Text;
            Helper.runQuery("update member set name = '" + name + "', email = '" + email + "', handphone = '" + handphone + "' where memberid = '" + selectedIndex + "'");
            getMemberRecord();
            clearMember();
        }

        private void dgvMember_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedIndex = (int)dgvMember.Rows[e.RowIndex].Cells["memberid"].Value;
            txtName.Text = dgvMember.Rows[e.RowIndex].Cells["name"].Value.ToString();
            txtEmail.Text = dgvMember.Rows[e.RowIndex].Cells["email"].Value.ToString();
            txtHP.Text = dgvMember.Rows[e.RowIndex].Cells["handphone"].Value.ToString();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (!isInputValid())
            {
                return;
            }
            string name, email, handphone;
            name = txtName.Text;
            email = txtEmail.Text;
            handphone = txtHP.Text;
            Helper.runQuery("insert into member (name, email, handphone, joindate) values ('" + name + "', '" + email + "', '" + handphone + "', cast (GETDATE() as Date))");
            getMemberRecord();
            clearMember();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Helper.runQuery("delete from member where memberid = '" + selectedIndex + "'");
            getMemberRecord();
            clearMember();
        }
        
        bool isInputValid()
        {
            bool res = true;
            if (!Helper.isEmailValid(txtEmail))
            {
                res = false;
            }
            if (Helper.isMt(textBoxes))
            {
                res = false;
            }
            if (Helper.isNumber(txtHP))
            {
                res = false;
            }
            return res;
        }
    }
}
