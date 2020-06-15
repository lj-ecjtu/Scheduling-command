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
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }

        int a=0, b=0, c=0, d=0, f=0;   //定义标志textBbox控件是否为空的变量，1表示非空，0表示空；
        private void Form7_Load(object sender, EventArgs e)
        {
            textBox3.PasswordChar = '*';
            textBox4.PasswordChar = '*';
            textBox5.PasswordChar = '*';
        }
        //当“工号”输入为空时，激活errorProvider1控件，显示错误描述字符串“不能为空”；
        //输入为空时，a=0,输入为非空时，a自动置为1；
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if(textBox1.Text=="")
            {
                a = 0;
                errorProvider1.SetError(textBox1, "不能为空");
            }
            else
            {
                errorProvider1.SetError(textBox1, "");
                a=1;
            }
        }

        private void textBox2_Validating_1(object sender, CancelEventArgs e)
        {
            if (textBox2.Text == "")
            {
                b = 0;
                errorProvider2.SetError(textBox2, "不能为空");
            }
            else
            {
                errorProvider2.SetError(textBox2, "");
                b = 1;
            }
        }

        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
              if(textBox3.Text=="")
            {
                c = 0;
                errorProvider3.SetError(textBox3, "不能为空");
            }
            else
            {
                errorProvider3.SetError(textBox3, "");
                 c=1;
            }
        }
        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            if (textBox4.Text == "")
            {
                d = 0;
                errorProvider4.SetError(textBox4, "不能为空");
            }
            else
            {
                errorProvider4.SetError(textBox4, "");
                d = 1;
            }
        }

        private void textBox5_Validating(object sender, CancelEventArgs e)
        {
            if (textBox5.Text == "")
            {
                f = 0;
                errorProvider5.SetError(textBox5, "不能为空");
            }
            else
            {
                errorProvider5.SetError(textBox5, "");
                f = 1;
            }
        }

 
        private void button1_Click(object sender, EventArgs e)
        {
            if (a + b + c + d + f == 5)    //判断a、b、c、d、f的和是否等于5，若为5表示5个文本框都为非空；
            {
                try
                {
                    MyMeans.SQLstr = "Select * from yonghu where WorkNumber =" + int.Parse(textBox1.Text.Trim()); 
                    SqlDataReader read = MyMeans.GgtDataReader(MyMeans.SQLstr);
                    //判断对应输入的工号是否查询到用户数据，如果查询到继续执行命令；
                    if (read.Read())
                    {
                        //判断对应输入工号的用户名和密码是否输入正确；
                        if (textBox2.Text.Trim() == read[0].ToString().Trim() & textBox3.Text.Trim() == read[2].ToString().Trim())
                        {
                            //判断两次新密码输入是否一致；
                            if (textBox4.Text == textBox5.Text)
                            {
                                MyMeans.SQLstr = "Update yonghu Set Password=" + int.Parse(textBox4.Text.Trim()) + "where WorkNumber=" + int.Parse(textBox1.Text.Trim());
                                MyMeans.ExecuteSql(MyMeans.SQLstr);
                                MessageBox.Show("修改成功");

                            }
                            else
                            {
                                MessageBox.Show("新密码输入不一致");
                            }
                        }
                        else
                        {
                            MessageBox.Show("用户信息输入错误");
                        }
                    }
                    //如果根据输入的“工号”没有查询到用户信息，则显示“用户信息输入错误”；
                    else
                    {
                        MessageBox.Show("用户信息输入错误");
                    }
                }
                catch { }
            }
            else
            {
                MessageBox.Show("请将信息输入完整");
            }
        }

      

        

      
    }
}
