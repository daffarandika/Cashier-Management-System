using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Kab_Desktop_2021
{
    public partial class Main : Form
    {
        string user { get; set; }
        public Main(string user)
        {
            this.user = user;
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ToolStripItem[] admin =
            {
                lOGINToolStripMenuItem
            };
            ToolStripItem[] chef =
            {
                lOGINToolStripMenuItem, dATAToolStripMenuItem, tRANSACTIONToolStripMenuItem, rEPORTToolStripMenuItem
            };
            ToolStripItem[] cashier = {
                lOGINToolStripMenuItem, dATAToolStripMenuItem, cHEFToolStripMenuItem, rEPORTToolStripMenuItem
            };
            ToolStripItem[] not_logged = {
                lOGOUTToolStripMenuItem, dATAToolStripMenuItem, cHEFToolStripMenuItem, rEPORTToolStripMenuItem, tRANSACTIONToolStripMenuItem
            };
            ToolStripItem[] to_disable = { };
            switch (user)
            {
                case "admin":
                    to_disable = admin;
                    break;
                case "chef":
                    to_disable = chef;
                    break;
                case "cashier":
                    to_disable = cashier;
                    break;
                default:
                    to_disable = not_logged;
                    break;
            }
            for (int i = 0; i < to_disable.Length; i++)
            {
                to_disable[i].Enabled = false;
            }
        }

        private void lOGINToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Login login = new Login();
            login.ShowDialog();
            Hide();
            Main main = new Main(Helper.role);
            main.ShowDialog();
            Close();
        }

        private void eMPLOYEEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Employee employee = new Employee();
            employee.ShowDialog();
        }

        private void eXITToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mEMBERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Member member = new Member();
            member.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void mENUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.ShowDialog();
        }



        private void vIEWORDERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewOrder vieworder = new ViewOrder();
            vieworder.ShowDialog();
        }

        private void oRDERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Order order = new Order("");
            order.ShowDialog();
        }



        private void pAYMENTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Payment payment = new Payment();
            payment.ShowDialog();
        }

        private void rEPORTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report report = new Report();
            report.ShowDialog();
        }


    }
}
