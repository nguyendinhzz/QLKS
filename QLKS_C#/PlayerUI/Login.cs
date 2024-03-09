using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace PlayerUI
{
    public partial class Login : Form
    {
        private SqlConnection conn=null;
        private string connKey = "Data Source=DESKTOP-410VARK;Initial Catalog=QLKS;Persist Security Info=True;User ID=sa;pwd=12345678";
        private int k;
        private string hoTen;
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
            {
                try
                {
                    conn = new SqlConnection(connKey);
                    conn.Open();
                    string query = "SELECT COUNT(*) from TaiKhoan WHERE taiKhoan = '" + textBox1.Text + "' and matKhau = '" + textBox2.Text + "'";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        //if ((int)command.ExecuteScalar()>0)
                        //    MessageBox.Show("Đăng nhập thành công!");
                        //else
                        //{
                        //    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
                        //    return;
                        //}
                    }
                    query = "SELECT hoTen from TaiKhoan WHERE taiKhoan = '" + textBox1.Text + "' and matKhau = '" + textBox2.Text + "'";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        hoTen = command.ExecuteScalar().ToString();
                        FormHome fh = new FormHome();
                        if(textBox1.Text.Substring(0,2)=="NV")
                            fh.pNhanVien=true;
                        else if (textBox1.Text.Substring(0, 2) == "QL")
                            fh.pQuanLy = true;
                        fh.inforLogin = this.hoTen;
                        fh.Show();
                        this.Hide();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            textBox1.Text=textBox1.Text.ToUpper();
        }
    }
}
