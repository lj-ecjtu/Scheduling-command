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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        public static string mllx = null;
        public static string mlbh = null;
        public static string slcz = null;
        public static string xl = null;
        public static string qsz = null;
        public static string jsz = null;
        public static string xsqd = null;
        public static string xszd = null;
        public static string xsyy = null;
        public static string xsxt = null;
        public static string kssj = null;
        public static string jssj = null;
        public static string xsnr = null;
 
        private void Form4_Load(object sender, EventArgs e)
        {
            MyMeans.SQLstr = "Select * from 临时限速命令";
            DataSet ds = null;
            ds = MyMeans.GetDataSet(MyMeans.SQLstr);
            dataGridView1.DataSource = ds.Tables[0];
            comboBox7.Items.Add("45");
            comboBox7.Items.Add("60");
            comboBox7.Items.Add("80");
            comboBox7.Items.Add("120");
            comboBox7.Items.Add("160");
            comboBox7.Items.Add("255");
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mlbh = textBox6.Text.Trim();
            mllx = comboBox1.Text.Trim();
            slcz = comboBox2.Text.Trim();
            xl = comboBox3.Text.Trim();
            qsz = comboBox4.Text.Trim();
            jsz = comboBox5.Text.Trim();
            xsqd = textBox1.Text.Trim() + "km" + textBox2.Text.Trim() + "m";
            xszd = textBox3.Text.Trim() + "km" + textBox4.Text.Trim() + "m";
            xsyy = textBox5.Text.Trim();
            if (checkBox1.CheckState == CheckState.Checked)
            {
                xsxt = "列控中心";
            }
            if (checkBox2.CheckState == CheckState.Checked)
            {
                xsxt += " RBC";
            }
            kssj = dateTimePicker1.Text;
            jssj = dateTimePicker2.Text;
            xsnr = comboBox7.Text;
            // 如果所有信息填写完整，则完成新建命令，将命令插入到数据库中
            if (mlbh != "" && mllx != "" && slcz != "" && xl != "" && qsz != "" && jsz != "" && xsqd != "" && xszd != "" && xsyy != "" && xsxt != "" && kssj != "" && jssj != "" && xsnr != "")
            {
                MyMeans.SQLstr="Insert Into 临时限速命令 Values('" + int.Parse(mlbh) + "','" + mllx + "','" + slcz + "','" + xl + "','" + qsz + "','" + jsz + "','" + xsqd + "','" + xszd + "','" + xsyy + "','" + xsxt + "','" + kssj + "','" + jssj + "','" + xsnr + "',null)";
                MyMeans.ExecuteSql(MyMeans.SQLstr);
                MyMeans.SQLstr = "Select * from 临时限速命令";
                DataSet ds = null;
                ds = MyMeans.GetDataSet(MyMeans.SQLstr);
                dataGridView1.DataSource = ds.Tables[0];
                comboBox1.Text = "";
                comboBox2.Text = "";
                comboBox3.Text = "";
                comboBox4.Text = "";
                comboBox5.Text = "";
                comboBox6.Text = "";
                comboBox7.Text = "";
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                checkBox1.CheckState= CheckState.Unchecked;
                checkBox2.CheckState = CheckState.Unchecked;


            }
            else
            {
                MessageBox.Show("请补全命令内容");
            }
        }
       

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //将选中新建命令的值赋给相应的变量；
            mlbh = dataGridView1.SelectedCells[0].Value.ToString().Trim();
            mllx = dataGridView1.SelectedCells[1].Value.ToString().Trim();
            slcz = dataGridView1.SelectedCells[2].Value.ToString().Trim();
            xl = dataGridView1.SelectedCells[3].Value.ToString().Trim();
            qsz = dataGridView1.SelectedCells[4].Value.ToString().Trim();
            jsz = dataGridView1.SelectedCells[5].Value.ToString().Trim();
            xsqd = dataGridView1.SelectedCells[6].Value.ToString().Trim();
            xszd = dataGridView1.SelectedCells[7].Value.ToString().Trim();
            xsyy = dataGridView1.SelectedCells[8].Value.ToString().Trim();
            xsxt = dataGridView1.SelectedCells[9].Value.ToString().Trim();
            kssj = dataGridView1.SelectedCells[10].Value.ToString().Trim();
            jssj = dataGridView1.SelectedCells[11].Value.ToString().Trim();
            xsnr = dataGridView1.SelectedCells[12].Value.ToString().Trim();
            //将选中新建命令的详细内容显示到正文编辑区；
            textBox6.Text = mlbh;
            comboBox1.Text = mllx;
            comboBox2.Text = slcz;
            comboBox3.Text = xl;
            comboBox4.Text = qsz;
            comboBox5.Text = jsz;
            //设置用分割字符串的字符数组和接受分割子字符串的数组；
            char[] fgqdzd = { 'k', 'm' };
            string[] jsqdzd = null;
            //将限速起点值赋给相应的控件；
            jsqdzd = xsqd.Split(fgqdzd);
            textBox1.Text = jsqdzd[0];
            textBox2.Text = jsqdzd[2];
            //将限速终点值赋给相应的控件；
            jsqdzd = xszd.Split(fgqdzd);
            textBox3.Text = jsqdzd[0];
            textBox4.Text = jsqdzd[2];
            textBox5.Text = xsyy;
            //将限速系统的情况显示在编辑区；
            char[] fgxsxt = { ' ' };
            string[] jsxsxt = null;
            jsxsxt = xsxt.Split(fgxsxt);
            foreach (string item in jsxsxt) 
            {
                if (item == "列控中心")
                {
                    checkBox1.CheckState = CheckState.Checked;
                }
                if (item == "RBC")
                {
                    checkBox2.CheckState = CheckState.Checked;
                }
            }
            dateTimePicker1.Text=kssj;
            dateTimePicker2.Text = jssj;
            comboBox7.Text = xsnr;
            
        }
        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form8 form8 = new Form8();
            form8.Show();
          
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MyMeans.SQLstr = "Select * from 临时限速命令";
            DataSet ds =MyMeans.GetDataSet(MyMeans.SQLstr);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void button4_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";
            comboBox4.Text = "";
            comboBox5.Text = "";
            comboBox6.Text = "";
            comboBox7.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            checkBox1.CheckState = CheckState.Unchecked;
            checkBox2.CheckState = CheckState.Unchecked;
        }

    }
}
