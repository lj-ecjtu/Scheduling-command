using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public static string Uers_Now;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "" && textBox2.Text.Trim() != "" && textBox3.Text.Trim() != "")
            {
                MyMeans.My_conn = new SqlConnection(MyMeans.Constr);
                MyMeans.My_conn.Open();
                SqlCommand com = new SqlCommand("select*from yonghu where Name='"+textBox1.Text.Trim()+"'and WorkNumber="+int.Parse(textBox2.Text.Trim())+"and Password="+int.Parse(textBox3.Text.Trim()), MyMeans.My_conn);
                SqlDataReader My_read = com.ExecuteReader();
                bool items = My_read.Read();
                if (items)
                {
                    Uers_Now = textBox1.Text.Trim();
                    MyMeans.My_conn.Dispose();
                    this.Hide();
                    Form2 form2 = new Form2();
                    form2.Show();
                }
                else
                {
                    MessageBox.Show("输入信息错误, 请重新输入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                    MyMeans.My_conn.Dispose();
                }
            }
            else
            {
                MessageBox.Show("请将登陆信息填写完整", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form7 form7 = new Form7();
            form7.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox3.PasswordChar = '*';
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Hide();     
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
}
