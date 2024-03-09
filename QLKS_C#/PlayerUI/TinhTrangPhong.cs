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

namespace PlayerUI
{
    public partial class TinhTrangPhong : Form
    {
        public TinhTrangPhong()
        {
            InitializeComponent();
        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel21_Paint(object sender, PaintEventArgs e)
        {

        }
        private string selectedMaP;
        private Color selectedColor;
        private void Panel_Click(object sender, EventArgs e)
        {
            Panel clickedPanel = (Panel)sender;
            selectedMaP=clickedPanel.Tag.ToString();
            selectedColor = clickedPanel.BackColor;
            if (selectedPanel != null && selectedPanel != clickedPanel)
            {
                using (Graphics g = selectedPanel.CreateGraphics())
                {
                    Pen pen = new Pen(Color.White, 4);
                    g.DrawRectangle(pen, 0, 0, selectedPanel.Width - 1, selectedPanel.Height - 1);
                }
            }
            using (Graphics g = clickedPanel.CreateGraphics())
            {
                Pen pen = new Pen(Color.Black, 4);
                g.DrawRectangle(pen, 0, 0, clickedPanel.Width - 1, clickedPanel.Height - 1);
            }
            selectedPanel = clickedPanel;
        }
        private Panel selectedPanel = null;
        private int single = 0;
        private int duo = 0;
        private int vip = 0;
        private int suite = 0;
        private void TinhTrangPhong_Load(object sender, EventArgs e)
        {
            timer1.Start();
            foreach (Control control in panel1.Controls)
            {
                if (control is Panel && control.Name.StartsWith("p"))
                {
                    control.Click += Panel_Click;
                }
            }
            try
            {
                conn = new SqlConnection(connKey);
                conn.Open();
                string query = "SELECT * FROM TinhTrangPhong";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();
                int i = 0;
                while (reader.Read())
                {
                    string maPhong = reader["maPhong"].ToString();
                    string loaiPhong = reader["loaiPhong"].ToString();
                    if (loaiPhong == "Std single")
                        single++;
                    else if (loaiPhong == "Std duo")
                        duo++;
                    else if (loaiPhong == "Vip")
                        vip++;
                    else if (loaiPhong == "Suite")
                        suite++;
                    string hoTen = reader["hoTen"].ToString();
                    string ngayDat="";
                    string ngayNhan="";
                    string ngayTra="";
                    if (!reader.IsDBNull(reader.GetOrdinal("ngayDat")))
                        ngayDat = ((DateTime)reader["ngayDat"]).ToString("hh:mm dd/MM/yyyy");
                    if (!reader.IsDBNull(reader.GetOrdinal("ngayNhan")))
                        ngayNhan = ((DateTime)reader["ngayNhan"]).ToString("hh:mm dd/MM/yyyy");
                    if (!reader.IsDBNull(reader.GetOrdinal("ngayTra")))
                        ngayTra = ((DateTime)reader["ngayTra"]).ToString("hh:mm dd/MM/yyyy");
                    i++;
                    Panel panel = (Panel)this.Controls.Find("p" + i.ToString(), true)[0];
                    panel.Paint += (paintSender, paintArgs) =>
                    {
                        panel.Tag = maPhong;
                        SizeF maPhongSize = paintArgs.Graphics.MeasureString(maPhong, new Font(panel.Font.FontFamily, panel.Font.Size + 2, FontStyle.Bold));
                        PointF maPhongLocation = new PointF((panel.Width - maPhongSize.Width) / 2 + 2, (panel.Height - maPhongSize.Height) / 2);
                        paintArgs.Graphics.DrawString(maPhong, new Font(panel.Font.FontFamily, panel.Font.Size + 2, FontStyle.Bold), Brushes.White, maPhongLocation);

                        Font boldFont = new Font(panel.Font.FontFamily,9f, FontStyle.Bold);
                        SizeF loaiPhongSize = paintArgs.Graphics.MeasureString(loaiPhong, boldFont);
                        PointF loaiPhongLocation = new PointF(0, (panel.Height - loaiPhongSize.Height) / 2);
                        paintArgs.Graphics.DrawString(loaiPhong, boldFont, new SolidBrush(Color.FromArgb(255, 20, 147)), loaiPhongLocation);

                        SizeF hoTenSize = paintArgs.Graphics.MeasureString(hoTen, panel.Font);
                        PointF hoTenLocation = new PointF((panel.Width - hoTenSize.Width) / 2, (panel.Height + maPhongSize.Height) / 2 - 2);
                        paintArgs.Graphics.DrawString(hoTen, panel.Font, Brushes.White, hoTenLocation);

                        SizeF ngayDatSize = paintArgs.Graphics.MeasureString(ngayDat, panel.Font);
                        PointF ngayDatLocation = new PointF(panel.Width - ngayDatSize.Width, 0);
                        paintArgs.Graphics.FillRectangle(Brushes.Yellow, new RectangleF(ngayDatLocation, ngayDatSize));
                        paintArgs.Graphics.DrawString(ngayDat, panel.Font, Brushes.Black, ngayDatLocation);

                        PointF ngayNhanLocation = new PointF(panel.Width - ngayDatSize.Width, ngayDatLocation.Y + ngayDatSize.Height);
                        paintArgs.Graphics.FillRectangle(Brushes.Yellow, new RectangleF(ngayNhanLocation, ngayDatSize));
                        paintArgs.Graphics.DrawString(ngayNhan, panel.Font, Brushes.Black, ngayNhanLocation);

                        paintArgs.Graphics.FillRectangle(new SolidBrush(Color.FromArgb	(210,105,30)), new RectangleF(0, panel.Height - 15, 200f,20f));
                        paintArgs.Graphics.DrawString(ngayTra, panel.Font, Brushes.Black, 20, panel.Height - 15);
                    };
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi: " + ex);
            }
            finally
            {
                conn.Close();
            }
        }

        private SqlConnection conn = null;
        private string connKey = "Data Source=DESKTOP-410VARK;Initial Catalog=QLKS;Persist Security Info=True;User ID=sa;pwd=12345678";
        private void updateTrangThai()
        {
            try
            {
                conn = new SqlConnection(connKey);
                conn.Open();
                string query = "SELECT * FROM TinhTrangPhong";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();
                List<string> queryUpdates= new List<string>(); // List để lưu trữ các câu lệnh query2
                int i = 0;
                while (reader.Read())
                {
                    i++;
                    string query2 = "UPDATE TinhTrangPhong SET tinhTrang=@tt Where maPhong='" + reader["maPhong"].ToString() + "'";
                    Panel panel = (Panel)this.Controls.Find("p" + i.ToString(), true)[0];
                    if (!reader.IsDBNull(reader.GetOrdinal("ngayDat")))
                    {
                        if (((DateTime)reader["ngayNhan"]) > DateTime.Now && (int)reader["checkIn"] == 0)
                        {
                            panel.BackColor = Color.Orange; //dat truoc
                            query2 = query2.Replace("@tt", "N'Đặt trước'");
                        }
                        else if (((DateTime)reader["ngayTra"]) > DateTime.Now && (int)reader["checkIn"] == 1)
                        {
                            query2 = query2.Replace("@tt", "N'Đang thuê'");//dangThue = true;
                            panel.BackColor = Color.RoyalBlue;
                        }
                        else if (((DateTime)reader["ngayTra"]) < DateTime.Now)
                        {
                            query2 = query2.Replace("@tt", "N'Trả phòng'");//traPhong = true;
                            panel.BackColor = Color.Violet;
                        }
                        else if (((DateTime)reader["ngayNhan"]) < DateTime.Now && (int)reader["checkIn"] == 0)
                        {
                            query2 = query2.Replace("@tt", "N'Quá hạn'");   //quaHan = true;
                            panel.BackColor = Color.Red;
                        }
                        queryUpdates.Add(query2); // Thêm câu lệnh query2 vào List
                    }
                    else
                    {
                        if (reader["tinhTrang"].ToString() == "Trống")
                            panel.BackColor = Color.LimeGreen;
                    }
                }
                
                reader.Close(); 
                foreach (string queryUp in queryUpdates)
                {
                    SqlCommand cmd = new SqlCommand(queryUp, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                fixDebug = true;
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void cacLoaiP(){
            textBox1.ReadOnly=true;
            textBox1.Text = single.ToString();
            textBox2.ReadOnly = true;
            textBox2.Text = duo.ToString();
            textBox3.ReadOnly = true;
            textBox3.Text = vip.ToString();
            textBox4.ReadOnly = true;
            textBox4.Text = suite.ToString();
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void p21_Paint(object sender, PaintEventArgs e)
        {

        }

        private void p5_Paint(object sender, PaintEventArgs e)
        {

        }

        private bool fixDebug;
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if(!fixDebug)
            updateTrangThai();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedColor == Color.Orange && DialogResult.Yes==MessageBox.Show("Thông tin KH và thanh toán hoàn tất?", "Xác nhận", MessageBoxButtons.YesNo))
            try
            {
                Console.WriteLine("check!");
                conn = new SqlConnection(connKey);
                conn.Open();
                string query="UPDATE tinhTrangPhong Set checkIn=1 Where maPhong='"+selectedMaP+"'";
                SqlCommand cmd= new SqlCommand(query,conn);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi: " + ex);
            }
            finally
            {
                conn.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (selectedColor == Color.RoyalBlue || selectedColor == Color.Violet)
                if(DialogResult.Yes == MessageBox.Show("{Phí phát sinh và dịch vụ đã được thanh toán?", "Xác nhận", MessageBoxButtons.YesNo))
                try
                {
                    Console.WriteLine("check2");
                    conn = new SqlConnection(connKey);
                    conn.Open();
                    string query = "UPDATE tinhTrangPhong Set checkIn=0,checkOut=1 Where maPhong='" + selectedMaP + "'";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xuất hóa đơn vào console?", "Xác nhận", MessageBoxButtons.YesNo);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Loi: " + ex);
                }
                finally
                {
                    conn.Close();
                }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (selectedColor == Color.Orange && DialogResult.Yes == MessageBox.Show("{Bạn có chắc chắn muốn hủy Đặt phòng trước cho khách hàng?", "Xác nhận", MessageBoxButtons.YesNo))
                    try
                    {
                        Console.WriteLine("check2");
                        conn = new SqlConnection(connKey);
                        conn.Open();
                        string query = "UPDATE tinhTrangPhong Set checkIn=0,checkOut=1 Where maPhong='" + selectedMaP + "'";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Xuất hóa đơn vào console?", "Xác nhận", MessageBoxButtons.YesNo);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Loi: " + ex);
                    }
                    finally
                    {
                        conn.Close();
                    }
        }
    }
}
