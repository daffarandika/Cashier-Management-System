using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace LKS_Kab_Desktop_2021
{
    internal class Helper
    {
        public static string phoneNumberPattern = "[0-9]+";
        public static ErrorProvider ep = new ErrorProvider();
        public static SqlConnection conn = new SqlConnection("Data Source=DESKTOP-GHNE639;Initial Catalog=Latihan_LKS_Kab;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        public static string role = "";
        public static string empID = "";
        public static void runQuery(string command)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(command, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static bool isNumber(TextBox box) {
            bool res = Regex.IsMatch(box.Text, phoneNumberPattern);
            if (!res)
            {
                ep.SetError(box, "input is invalid");            
            }
            return res;
        }
        public static bool isMt(TextBox[] textBoxes)
        {
            
            bool res = false;
            foreach (TextBox box in textBoxes)
            {
                if (box.Text == "")
                {
                    ep.SetError(box, "this field cannot be empty");
                    res = true;
                } else
                {
                    ep.SetError(box, "");
                    res = false;
                }
            }
            return res;
        }
        public static string toSha256(string input)
        {

            SHA256 sHA256 = SHA256.Create();
            byte[] res = sHA256.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in res)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        public static void getRecord(DataGridView dgv, string command)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(command, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dgv.DataSource = dt;
            dgv.ReadOnly = true;
            conn.Close();
        }
        public static void clearTxt(TextBox[] textBoxes)
        {
            foreach (TextBox textBox in textBoxes)
            {
                textBox.Text = "";
            }
        }
        public static bool isEmailValid(TextBox box)
        {
            bool res = true;
            try
            {
                res = box.Text.Contains('@');
            }
            catch
            {
                res = false;
                ep.SetError(box, "invalid email address");
            }
            return res;
        }
        public static string searchDateInTable(string tableName)
        {
            conn.Open();
            string res = "";
            SqlCommand cmd = new SqlCommand("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME LIKE '%date%' and TABLE_NAME = @tableName", conn);
            cmd.Parameters.AddWithValue("@tableName", tableName);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                res = reader.GetString(0);
            }
            conn.Close();
            return res;
        }
        public static string generateOrderID()
        {
            string init_date = DateTime.Now.ToString("yyyyMM") + "0000"; //year, month, and a bunch of zeros
            string num_orders = "";
            Helper.conn.Open();
            SqlCommand cmd = new SqlCommand("select max(orderid) as max from headorder", Helper.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                num_orders = reader["max"].ToString();
            }
            Helper.conn.Close();
            num_orders = num_orders.Remove(0,5); //the number a bunch of zeros (how many order has this restaurant had since this month)

            int date_6 = Convert.ToInt32(init_date) + Convert.ToInt32(num_orders) + 1;
            return date_6.ToString();
        }
        public static string monthToText(string month_num)
        {
            string res;
            switch (month_num)
            {
                case "01":
                    res = "JANUARY";
                    break;
                case "02":
                    res = "FEBRUARY";
                    break;
                case "03":
                    res = "MARCH";
                    break;
                case "04":
                    res = "APRIL";
                    break;
                case "05":
                    res = "MAY";
                    break;
                case "06":
                    res = "JUNE";
                    break;
                case "07":
                    res = "JULY";
                    break;
                case "08":
                    res = "AUGUST";
                    break;
                case "09":
                    res = "SEPTEMBER";
                    break;
                case "10":
                    res = "OCTOBER";
                    break;
                case "11":
                    res = "NOVEMBER";
                    break;
                case "12":
                default:
                    res = "DECEMBER";
                    break;
            }
            return res;
        }
    }
}
