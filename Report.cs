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
    public partial class Report : Form
    {
        DataTable dt = new DataTable();
        public Report()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            fillChart();
            fillDT();
            
            cmbDateFrom.DataSource = dt;
            cmbDateFrom.ValueMember = "monthNum";
            cmbDateFrom.DisplayMember = "month";

            cmbDateTo.BindingContext = new BindingContext();
            cmbDateTo.DataSource = dt;
            cmbDateTo.ValueMember = "monthNum";
            cmbDateTo.DisplayMember = "month";
            
        }
        void fillDT()
        {
            dt.Columns.Add("monthNum");
            dt.Columns.Add("month");
            dt.Rows.Clear();
            dt.Rows.Add("01", "JANUARY");
            dt.Rows.Add("02", "FEBRUARY");
            dt.Rows.Add("03", "MARCH");
            dt.Rows.Add("04", "APRIL");
            dt.Rows.Add("05", "MAY");
            dt.Rows.Add("06", "JUNE");
            dt.Rows.Add("07", "JULY");
            dt.Rows.Add("08", "AUGUST");
            dt.Rows.Add("09", "SEPTEMBER");
            dt.Rows.Add("10", "OCTOBER");
            dt.Rows.Add("11", "NOVEMBER");
            dt.Rows.Add("12", "DECEMBER");
        }

        void getReportRecord(string from, string to)
        {
            Helper.conn.Open();
            SqlCommand cmd = new SqlCommand("select * from income where month >= '" + from + "' and month <= '" + to + "'", Helper.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("month");
            dt.Columns.Add("income");
            dt.Rows.Clear();
            while (reader.Read()) {
                dt.Rows.Add(Helper.monthToText(reader["month"].ToString()), reader["income"]);
            }
            Helper.conn.Close();
            dgvReport.DataSource = dt;
        }
        void fillChart()
        {
            chart1.Series["Income"].Points.Clear();
            try
            {
                foreach (DataGridViewRow row in dgvReport.Rows)
                {
                    chart1.Series["Income"].Points.AddXY(row.Cells["month"].Value, row.Cells["income"].Value);
                }
            } catch (Exception e)
            {
                //return;
            }
            

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            getReportRecord(cmbDateFrom.SelectedValue.ToString(), cmbDateTo.SelectedValue.ToString());
            fillChart();
        }
    }
}
