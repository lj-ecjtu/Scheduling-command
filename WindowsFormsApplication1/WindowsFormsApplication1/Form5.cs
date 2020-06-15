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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();

        }
        
        private void Form5_Load(object sender, EventArgs e)
        {
            BindInfo();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)   //将当前选中的信息显示到界面右侧相应的文本框中
        {
            textBox1.Text = dataGridView1.SelectedCells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedCells[1].Value.ToString();
            richTextBox1.Text = dataGridView1.SelectedCells[2].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)         //点击“添加”按钮，添加新的调度命令模板到数据库中
        {
            Form3 form3 = new Form3();
            form3.Show(); 
        }
    
        private void button2_Click(object sender, EventArgs e)          //点击“删除”按钮，删除选中的调度命令模板
        {
            MyMeans.linq = new linqtosqlClassDataContext(MyMeans.Constr);
            string strLeiXing = dataGridView1.SelectedCells[1].Value.ToString();
            var result = from items in MyMeans.linq.mlmoban
                         where items.命令类型 == strLeiXing
                         select items;
            MyMeans.linq.mlmoban.DeleteAllOnSubmit(result);          //删除模板信息
            MyMeans.linq.SubmitChanges();                            //提交操作
            MessageBox.Show("删除成功");
            BindInfo();
            textBox1.Text = "";
            textBox2.Text = "";
            richTextBox1.Text = "";
         }
        private void BindInfo()                                  //获取所有模板信息，并显示到DataGridView1控件上；
        {
            MyMeans.linq = new linqtosqlClassDataContext(MyMeans.Constr);
            var result = from info in MyMeans.linq.mlmoban
                         select new
                         {
                             模板编号 = info.模板编号,
                             命令类型 = info.命令类型,
                             命令内容 = info.命令内容,
                         };
            dataGridView1.DataSource = result;                      //对dataGridView控件进行数据绑定
        }

        private void button4_Click(object sender, EventArgs e)      //点击“关闭”按钮，关闭命令模块界面
        {
            MyMeans.SQLstr = "select*from mlmoban";
            DataSet ds= MyMeans.GetDataSet(MyMeans.SQLstr);
            dataGridView1.DataSource = ds.Tables[0];
            richTextBox1.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
            {
                MessageBox.Show("请选择要修改的命令模板","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            MyMeans.linq = new linqtosqlClassDataContext(MyMeans.Constr);
            var result = from info in MyMeans.linq.mlmoban
                         where info.模板编号 == textBox1.Text
                         select info;
            foreach (var items in result)
            {
                items.命令类型 = textBox2.Text;
                items.命令内容 = richTextBox1.Text;
                MyMeans.linq.SubmitChanges();
            }
            MessageBox.Show("修改成功");
            BindInfo();
        }

    }
}
