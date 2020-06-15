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
    public partial class Form6 : Form
    {
        string ChaXun_ml = null;
        int ChaXun_mlbh;
        string ChaXun_ddyxm = null;
        int a=0,b=0,c=0;
        public Form6()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ChaXun_ml = "daifaml";
            MyMeans.My_conn = new SqlConnection(MyMeans.Constr);
            MyMeans.My_conn.Open();
            SqlCommand com =new SqlCommand("Select*from daifaml",MyMeans.My_conn);
            SqlDataAdapter dataAdapter =new SqlDataAdapter();
            dataAdapter.SelectCommand=com;
            DataSet dataset =new DataSet();
            dataAdapter.Fill(dataset);
            dataGridView1.DataSource=dataset.Tables[0];
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Text = "2000/01/01  00:00:00";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChaXun_ml = "fasongml";
            groupBox5.Text = "发送命令";
            MyMeans.My_conn = new SqlConnection(MyMeans.Constr);
            MyMeans.My_conn.Open();
            SqlCommand com = new SqlCommand("Select*from fasongml", MyMeans.My_conn);
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = com;
            DataSet dataset = new DataSet();
            dataAdapter.Fill(dataset);
            dataGridView1.DataSource = dataset.Tables[0];   
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChaXun_ml = "yifaml";
            MyMeans.My_conn = new SqlConnection(MyMeans.Constr);
            MyMeans.My_conn.Open();
            SqlCommand com = new SqlCommand("Select*from yifaml", MyMeans.My_conn);
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = com;
            DataSet dataset = new DataSet();
            dataAdapter.Fill(dataset);
            dataGridView1.DataSource = dataset.Tables[0];   
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox3.Text = dataGridView1.SelectedCells[0].Value.ToString();
            textBox6.Text = dataGridView1.SelectedCells[1].Value.ToString();
            textBox4.Text = dataGridView1.SelectedCells[2].Value.ToString();
            textBox5.Text = dataGridView1.SelectedCells[3].Value.ToString();
            dateTimePicker3.Text = dataGridView1.SelectedCells[4].Value.ToString();
            richTextBox1.Text = dataGridView1.SelectedCells[5].Value.ToString();
            textBox7.Text = dataGridView1.SelectedCells[6].Value.ToString();
            string StrNumber = dataGridView1.SelectedCells[0].Value.ToString();
            DataSet ds;
            ds = MyMeans.GetDataSet("Select 受令单位,签收状态,签收人,签收时间,签收结果 from 受令车站对应表 where 命令编号=" + int.Parse(StrNumber) );
            dataGridView2.DataSource = ds.Tables[0];              
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked)
            {
                a = 1;
                ChaXun_mlbh = int.Parse(textBox1.Text.Trim());
            }
            if (checkBox2.CheckState == CheckState.Checked)
            {
                b = 1;
                ChaXun_ddyxm = textBox2.Text.Trim();
            }
            if (checkBox3.CheckState == CheckState.Checked)
            {
                c = 1;
            }
            if (a == 1 & b == 0 & c == 0)
            {
                MyMeans.SQLstr = "Select*from " + ChaXun_ml + " where 命令编号=" + ChaXun_mlbh ;
                DataSet dataset = MyMeans.GetDataSet(MyMeans.SQLstr);
                dataGridView1.DataSource = dataset.Tables[0];
                a = 0;
            }
            if (a == 0 & b == 1 & c == 0)
            {
                MyMeans.SQLstr = "Select*from " + ChaXun_ml + " where 调度员姓名='" + ChaXun_ddyxm + "'";
                DataSet dataset = MyMeans.GetDataSet(MyMeans.SQLstr);
                dataGridView1.DataSource = dataset.Tables[0];
                b = 0;
            }
            if (a == 0 & b == 0 & c == 1)
            {
                MyMeans.SQLstr = "Select*from " + ChaXun_ml+" where 发令时间>='"+dateTimePicker1.Value.ToString()+"'and 发令时间<='"+dateTimePicker2.Value.ToString()+"'";
                DataSet dataset = MyMeans.GetDataSet(MyMeans.SQLstr);
                dataGridView1.DataSource = dataset.Tables[0];
                c = 0;
            }
            if (a == 1 & b == 1 & c == 0)
            {
                MyMeans.SQLstr = "Select*from " + ChaXun_ml + " where 命令编号=" + ChaXun_mlbh + "and 调度员姓名='" + ChaXun_ddyxm + "'";
                DataSet dataset = MyMeans.GetDataSet(MyMeans.SQLstr);
                dataGridView1.DataSource = dataset.Tables[0];
                a = 0;
                b = 0;
            }
            if (a == 1 & b == 0 & c == 1)
            {
                MyMeans.SQLstr = "Select*from " + ChaXun_ml + " where 命令编号=" + ChaXun_mlbh + "and 发令时间>='" + dateTimePicker1.Value.ToString() + "'and 发令时间<='" + dateTimePicker2.Value.ToString() + "'"; ;
                DataSet dataset = MyMeans.GetDataSet(MyMeans.SQLstr);
                dataGridView1.DataSource = dataset.Tables[0];
                a = 0;
                c = 0;
            }
            if (a == 0 & b == 1 & c == 1)
            {
                MyMeans.SQLstr = "Select*from " + ChaXun_ml + " where 调度员姓名='" + ChaXun_ddyxm + "'and 发令时间>='" + dateTimePicker1.Value.ToString() + "'and 发令时间<='" + dateTimePicker2.Value.ToString() + "'"; ;
                DataSet dataset = MyMeans.GetDataSet(MyMeans.SQLstr);
                dataGridView1.DataSource = dataset.Tables[0];
                b = 0;
                c = 0;
            }
            if (a == 1 & b == 1 & c == 1)
            {
                MyMeans.SQLstr = "Select*from " + ChaXun_ml + " where 命令编号=" + ChaXun_mlbh + "and 调度员姓名='"+ChaXun_ddyxm+"'and 发令时间>='" + dateTimePicker1.Value.ToString() + "'and 发令时间<='" + dateTimePicker2.Value.ToString() + "'"; ;
                DataSet dataset = MyMeans.GetDataSet(MyMeans.SQLstr);
                dataGridView1.DataSource = dataset.Tables[0];
                a = 0;
                b = 0;
                c = 0;
            }
        }

    }
}
