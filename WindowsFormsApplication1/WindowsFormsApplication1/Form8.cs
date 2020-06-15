using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }

        bool a = false;
        bool b = false;
        bool c = false;
        bool d = false;
        bool f = false;
        bool g = false;
        bool h = false;
        bool i = false;
        bool j = false;
        bool k = false;
        bool l = false;
        bool m = false;
        bool n = false;
        private void Form8_Load(object sender, EventArgs e)
        {
            string newstr = null;
            newstr = string.Format("命令类型：{0}    命令编号：{1}", Form4.mllx, Form4.mlbh);
            listBox1.Items.Add(newstr);
            newstr = string.Format("受令车站：{0}    线路：{1}", Form4.slcz, Form4.xl);
            listBox1.Items.Add(newstr);
            newstr = string.Format("起始站：{0}      结束站：{1}", Form4.qsz, Form4.jsz);
            listBox1.Items.Add(newstr);
            listBox1.Items.Add("限速起点：" + Form4.xsqd);
            listBox1.Items.Add("限速终点：" + Form4.xszd);
            listBox1.Items.Add("限速原因：" + Form4.xsyy);
            listBox1.Items.Add("限速系统：" + Form4.xsxt);
            listBox1.Items.Add("开始时间：" + Form4.kssj);
            listBox1.Items.Add("结束时间：" + Form4.jssj);
            listBox1.Items.Add("限速值：" + Form4.xsnr);
            comboBox7.Items.Add("45");
            comboBox7.Items.Add("60");
            comboBox7.Items.Add("80");
            comboBox7.Items.Add("120");
            comboBox7.Items.Add("160");
            comboBox7.Items.Add("255");
            

        }

        private void button1_Click(object sender, EventArgs e)
        {   //得到现在限速系统的文本，赋给变量XSXT；
            string XSXT = null;
            if (checkBox1.CheckState == CheckState.Checked)
            {
                XSXT = "列控中心";
            }
            if (checkBox2.CheckState == CheckState.Checked)
            {
                XSXT += " RBC";
            }
            //逐一比较指定限速命令的各个信息与现在编辑的信息是否一致；
            if (Form4.mlbh == textBox6.Text.Trim())
            {
                a = true;
            }
            else
            {
                a = false;
            }
            if (Form4.mllx == comboBox1.Text.Trim())
            {
                b = true;
            }
            else
            {
                b = false;
            }
            if (Form4.slcz == comboBox2.Text.Trim())
            {
                c = true;
            }
            else
            {
                c = false;
            }
            if (Form4.xl == comboBox3.Text.Trim())
            {
                d = true;
            }
            else
            {
                d = false;
            }
            if (Form4.qsz == comboBox4.Text.Trim())
            {
                f = true;
            }
            else
            {
                f = false;
            }
            if (Form4.jsz == comboBox5.Text.Trim())
            {
                g = true;
            }
            else
            {
                g = false;
            }
            if (Form4.xsqd == textBox1.Text.Trim() + "km" + textBox2.Text.Trim() + "m")
            {
                h = true;
            }
            else
            {
                h = false;
            }
            if (Form4.xszd == textBox3.Text.Trim() + "km" + textBox4.Text.Trim() + "m")
            {
                i = true;
            }
            else
            {
                i = false;
            }
            if (Form4.xsyy == textBox5.Text.Trim())
            {
                j = true;
            }
            else
            {
                j = false;
            }
            if (Form4.xsxt == XSXT)
            {
                k = true;
            }
            else
            {
                k = false;
            }
            if (Form4.kssj == dateTimePicker1.Text)
            {
                l = true;
            }
            else
            {
                l = false;
            }
            if (Form4.jssj == dateTimePicker2.Text)
            {
                m = true;
            }
            else
            {
                m = false;
            }
            if (Form4.xsnr == comboBox7.Text.Trim())
            {
                n = true;
            }
            else
            {
                n = false;
            }
            if(a==true&b==true&c==true&d==true&f==true&g==true&h==true&i==true&j==true&k==true&n==true)
            {
                MyMeans.SQLstr = "Update 临时限速命令 Set 是否检验='检验通过' where 命令编号='" + int.Parse(Form4.mlbh) + "'";
                MyMeans.ExecuteSql(MyMeans.SQLstr);
                MessageBox.Show("检验通过");
            }
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

      

    }
}
