using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;

namespace LKS_Kab_Desktop_2021
{
    public partial class Menu : Form
    {
        string full_image_path;
        TextBox[] textBoxes;
        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            getMenuRecord();
            textBoxes = new TextBox[]
            {
                txtName, txtPhoto, txtPrice
            };
        }
         void getMenuRecord()
         {
            Helper.getRecord(dgvMenu, "select * from menu");
         }

        private void dgvMenu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = dgvMenu.CurrentRow.Cells["name"].Value.ToString();
            txtPrice.Text = dgvMenu.CurrentRow.Cells["price"].Value.ToString();
            txtPhoto.Text = dgvMenu.CurrentRow.Cells["photo"].Value.ToString();
            pbMenu.Image = Image.FromFile(@"C:\Users\ASUS\source\repos\LKS_Kab_Desktop_2021\Images\" + txtPhoto.Text);
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (!isInputValid())
            {
                return;
            }
            string name, price, photo;
            name = txtName.Text;
            price = txtPrice.Text;
            photo = txtPhoto.Text;
            Helper.runQuery("insert into menu (name, price, photo) values ('" + name + "', '" + price + "', '" + photo + "')");
            File.Copy(full_image_path,
                      Path.Combine(@"C:\Users\ASUS\source\repos\LKS_Kab_Desktop_2021\Images\", Path.GetFileName(full_image_path)),
                      true);
            getMenuRecord();
            clearMenuTexts();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files (*.png; *.jpg; *.jpeg)|*.png; *.jpg; *.jpeg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pbMenu.Image = new Bitmap(ofd.FileName);
                txtPhoto.Text = ofd.SafeFileName;
                full_image_path = ofd.FileName;
            }
        }
        void clearMenuTexts()
        {
            Helper.clearTxt(textBoxes);
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            string menuid ,name, price, photo;
            menuid = dgvMenu.CurrentRow.Cells["menuid"].Value.ToString();
            name = txtName.Text;
            price = txtPrice.Text;
            photo = txtPhoto.Text;
            Helper.runQuery("update menu set name = '" + name + "', price = '" + price + "', photo = '" + photo + "' where menuid = '" + menuid + "'");
            getMenuRecord();
            clearMenuTexts();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string menuid = dgvMenu.CurrentRow.Cells["menuid"].Value.ToString();
            Helper.runQuery("delete from menu where menuid = '" + menuid + "'");
            getMenuRecord();
            clearMenuTexts();
        }
        bool isInputValid()
        {
            bool res = true;
            if (Helper.isMt(textBoxes))
            {
                res = false;
            }
            if (!Helper.isNumber(txtPrice))
            {
                res = false;
            }
            //txtName.Text = Helper.isNumber(txtHP).ToString();
            return res;
        }
    }
}
