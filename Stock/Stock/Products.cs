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

namespace Stock
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0; //comboBox 처음값을 보여줌(Active)
            LoadData(); 
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source = HP_07; Initial Catalog = test; Integrated Security = True");
            //Isert logic
            con.Open();
            bool status = false;
            if(comboBox1.SelectedIndex == 0)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            var SqlQuery = "";
            
            if (IfProductsExists(con, txt_code.Text))
            {
                SqlQuery = @"UPDATE [Products] SET [ProductName] = '" + txt_name.Text + "',[ProductStatus] = '" + status + "' WHERE [ProductCode] = '" + txt_code.Text + "'";

            }
            else
            {
                SqlQuery = @"INSERT INTO [test].[dbo].[Products]([ProductCode],[Productname],[ProductStatus]) VALUES
                        ('" + txt_code.Text + "','" + txt_name.Text + "','" + status + "')";
            }
            

            SqlCommand cmd = new SqlCommand(SqlQuery,con);
            cmd.ExecuteNonQuery();
            con.Close();

            //Reading Data
            LoadData();//지금껏 데이터를 girdview에 남겨둠
        }

        private bool IfProductsExists(SqlConnection con, string productcode)
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select 1 From [Products] WHERE [ProductCode]= '" + productcode + "'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
                 return true;
            else
                 return false;
     
        
        }
        public void LoadData()
        {
            SqlConnection con = new SqlConnection("Data Source = HP_07; Initial Catalog = test; Integrated Security = True");
            SqlDataAdapter sda = new SqlDataAdapter("Select * From [test].[dbo].[Products]", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["ProductCode"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["ProductName"].ToString();
                if ((bool)item["ProductStatus"])
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Active";
                }
                else
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Deactive";
                }

            }
        }
        
        //gridview에서 더블클릭시 textbox에 해당 code와name 보여주기 
        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txt_code.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txt_name.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString()=="Active")
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.SelectedIndex = 1;
            }
            
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source = HP_07; Initial Catalog = test; Integrated Security = True");
            var SqlQuery = "";

            if (IfProductsExists(con, txt_code.Text))
            {
                con.Open();
                SqlQuery = @"DELETE FROM [Products] WHERE [ProductCode] = '" + txt_code.Text + "'";
                SqlCommand cmd = new SqlCommand(SqlQuery, con);
                cmd.ExecuteNonQuery();
                con.Close();

            }
            else
            {
                MessageBox.Show("Record Not Exists");
            }
            
            //Reading Data
            LoadData();//지금껏 데이터를 girdview에 남겨둠
        }
    }
}
