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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            txt_id.Text = "";
            txt_pw.Text = "";
            txt_pw.Clear();
            txt_id.Focus();//txt_id로 커서 이동
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            //check login username and password
            SqlConnection con = new SqlConnection("Data Source = HP_07; Initial Catalog = test; Integrated Security = True");
            SqlDataAdapter sda = new SqlDataAdapter(@"SELECT * FROM [test].[dbo].[Login] Where UserName = '"+txt_id.Text+"' and Password = '"+txt_pw.Text+"'",con);

            DataTable dt = new DataTable();
            sda.Fill(dt);
            if(dt.Rows.Count ==1)
            {
                this.Hide();
                StockMain main = new StockMain();
                main.Show();
            }
            else
            {
                MessageBox.Show("등록되지않은 아이디와 비밀번호입니다.", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btn_Clear_Click(sender, e);//클리어처리
            }
        }
    }
}
