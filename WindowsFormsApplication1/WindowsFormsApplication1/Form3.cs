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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            int Temple_BH=1;
            MyMeans.SQLstr="select* from mlmoban";
            SqlDataReader Sdr=MyMeans.GgtDataReader(MyMeans.SQLstr);
            while(Sdr.Read())
            {
                if(int.Parse(Sdr[0].ToString())>Temple_BH)
                {
                    Temple_BH=int.Parse(Sdr[0].ToString());
                }
            }
            Temple_BH++;
            textBox1.Text=Temple_BH.ToString();
            textBox2.Focus();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyMeans.SQLstr = "Insert into mlmoban values ('" + textBox1.Text.Trim() + "','" + textBox2.Text.Trim() + "','" + richTextBox1.Text.Trim() + "')";
            MyMeans.ExecuteSql(MyMeans.SQLstr);
            MessageBox.Show("增加成功");
            Form5 form5 =new Form5();
            textBox2.Text = "";
            richTextBox1.Text = "";
            int Temple_BH = 1;
            MyMeans.SQLstr = "select* from mlmoban";
            SqlDataReader Sdr = MyMeans.GgtDataReader(MyMeans.SQLstr);
            while (Sdr.Read())
            {
                if (int.Parse(Sdr[0].ToString()) > Temple_BH)
                {
                    Temple_BH = int.Parse(Sdr[0].ToString());
                }
            }
            Temple_BH++;
            textBox1.Text = Temple_BH.ToString();
            textBox2.Focus();

        }
    }
}
