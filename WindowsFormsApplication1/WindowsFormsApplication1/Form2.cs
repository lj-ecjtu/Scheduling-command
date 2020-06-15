using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {

        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static int port = 2000;
        static string host = "192.168.43.40";
        static  IPAddress ip = IPAddress.Parse(host);
        IPEndPoint IPP = new IPEndPoint(ip, port);
        Thread jieshouMsg = null;
        int Number_ML=6001;
        int isChongxinBj = 0;    //在保存调度命令时，0表示不是重新编辑，1表示是重新编辑；

        public Form2()
        {
            InitializeComponent();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            //设置为允许跨线程调用空间
            Control.CheckForIllegalCrossThreadCalls = false;

            DataSet ds;
            ds = MyMeans.GetDataSet("Select*from daifaml");
            dataGridView1.DataSource=ds.Tables[0];
            MyMeans.Close();
            ds = MyMeans.GetDataSet("Select*from fasongml");
            dataGridView2.DataSource = ds.Tables[0];
            MyMeans.Close();
            ds = MyMeans.GetDataSet("Select*from yifaml");
            dataGridView3.DataSource = ds.Tables[0];
            MyMeans.Close();
            ds = MyMeans.GetDataSet("Select*from shoulinglb");
            dataGridView4.DataSource = ds.Tables[0];
            MyMeans.Close();
 
            MyMeans.linq = new linqtosqlClassDataContext(MyMeans.Constr);
            var result = from items in MyMeans.linq.mlmoban
                         select new
                         {
                             Number = items.模板编号,
                             LeiXing = items.命令类型,
                             NeiRong = items.命令内容,
                         };
            foreach (var item in result)
            {
                comboBox1.Items.Add(item.LeiXing);
            }
            SqlDataReader Sdr;
            MyMeans.SQLstr = "select*from changyongch";
            Sdr=MyMeans.GgtDataReader(MyMeans.SQLstr);
            while (Sdr.Read())
            {
                listBox1.Items.Add(Sdr[0]);
            }

            try //连接服务器，若连接成功，通信状态显示为“通信正常”；
            {
                socket.Connect(IPP);
                string SendStr = "行调系统";
                byte[] Bs = Encoding.UTF8.GetBytes(SendStr);
                socket.Send(Bs, Bs.Length, 0);
                label7.Text = "通信正常";
                jieshouMsg = new Thread(new ThreadStart(RecMsg));
                jieshouMsg.IsBackground = true;
                jieshouMsg.Start();
            }
            catch
            {
                
            }
            textBox4.Text = Form1.Uers_Now;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (richTextBox1.SelectionLength > 0)
            {
                richTextBox1.SelectionStart = richTextBox1.SelectionStart + richTextBox1.SelectionLength;
            }
            richTextBox1.SelectedText = listBox1.SelectedItem.ToString().Trim();
            richTextBox1.Focus();//使richTextBox1获得光标焦点；
            
        
        }
       
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
         
           
        }

        private void button8_Click(object sender, EventArgs e)
        {
            
            Form4 form4 = new Form4();
            form4.Show();
        }

   
        
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入要添加的数据");
            }
            else
            {
                MyMeans.SQLstr="Insert into changyongch values('"+textBox1.Text+"')";
                MyMeans.ExecuteSql(MyMeans.SQLstr);
                listBox1.Items.Add(textBox1.Text);
                textBox1.Text = "";
            }   
        }
        
       
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要删除的数据");
            }
            else
            {
                MyMeans.SQLstr = "delete from changyongch where 常用词汇='" + listBox1.SelectedItem + "'";
                MyMeans.ExecuteSql(MyMeans.SQLstr);
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)           //“调度命令模板”按钮
        {
            
            Form5 form5 = new Form5();
            form5.Show();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)         //“调度命令查询”按钮
        {
            
            Form6 form6 = new Form6();
            form6.Show();
        }

        private void button4_Click(object sender, EventArgs e)            //“保存”按钮，点击将当前调度命令保存到待发箱
        {
            int a = 0, b = 0, c = 0, d = 0, f = 0, g = 0,h=0;
            string SLchezhan = null;
            for (int i = 0; i < dataGridView4.Rows.Count; i++)
            {

                DataGridViewCheckBoxCell dgvCheckBoxCell = dataGridView4.Rows[i].Cells[0] as DataGridViewCheckBoxCell;//获取DataGridViewCheckBoxCell对象；

                if (dgvCheckBoxCell.Value == "1")               //判断该DataGridViewCheckBoxCell是否选中；
                {
                    SLchezhan += (dataGridView4.Rows[i].Cells[1].Value.ToString().Trim() + "#");
                   
                }
            }
            if (textBox2.Text.Trim() != "")
            {
                a = 1;
            }
            if (comboBox1.Text.Trim() != "")
            {
                b = 1;
            }
            if (textBox4.Text.Trim() != "")
            {
                c = 1;
            }
            if (textBox5.Text.Trim() != "")
            {
                d = 1;
            }
            if (dateTimePicker1.Text != "")
            {
                f = 1;
            }
            if (richTextBox1.Text.Trim() != "")
            {
                g = 1;
            }
            if (SLchezhan != "")
            {
                h = 1;
            }
            if (a == 1 & b == 1 & c == 1 & d == 1 & f == 1 & g == 1 & h == 1)
            {
                if (isChongxinBj == 0)
                {
                    MyMeans.SQLstr = "Insert into daifaml(命令编号,命令类型,调度员姓名,发令单位,起草时间,命令内容,当前状态)values(" + int.Parse(textBox2.Text.Trim()) + ",'" + comboBox1.Text.Trim() + "','" + textBox4.Text.Trim() + "','" + textBox5.Text.Trim() + "','" + dateTimePicker1.Text + "','" + richTextBox1.Text.Trim() + "','未发送')";// 注意圆括号中的 ， 用英文形式
                    MyMeans.ExecuteSql(MyMeans.SQLstr);

                    for (int i = 0; i < dataGridView4.Rows.Count; i++)
                    {

                        DataGridViewCheckBoxCell dgvCheckBoxCell = dataGridView4.Rows[i].Cells[0] as DataGridViewCheckBoxCell;//获取DataGridViewCheckBoxCell对象；

                        if (dgvCheckBoxCell.Value == "1")               //判断该DataGridViewCheckBoxCell是否选中；
                        {
                            MyMeans.SQLstr = "Insert into 受令车站对应表(命令编号,受令单位)values(" + int.Parse(textBox2.Text.Trim()) + ",'" + dataGridView4.Rows[i].Cells[1].Value.ToString().Trim() + "')";
                            MyMeans.ExecuteSql(MyMeans.SQLstr);
                        }
                    }
                    DataSet ds = MyMeans.GetDataSet("Select*from daifaml");
                    dataGridView1.DataSource = ds.Tables[0];
                    textBox2.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    comboBox1.Text = "";
                    richTextBox1.Text = "";
                    for (int i = 0; i < dataGridView4.Rows.Count; i++)
                    {

                        dataGridView4.Rows[i].Cells[0].Value = "0";

                    }
                }
                else
                {
                    MyMeans.SQLstr = "delete from 受令车站对应表 where 命令编号=" + int.Parse(textBox2.Text.Trim());
                    MyMeans.ExecuteSql(MyMeans.SQLstr);     //删除该条调度命令原来的受令车站信息；
                    MyMeans.SQLstr = "update daifaml set 命令类型='" + comboBox1.Text.Trim() + "',调度员姓名='" + textBox4.Text.Trim() + "',起草时间='" + dateTimePicker1.Text.Trim() + "',命令内容='" + richTextBox1.Text.Trim() + "'where 命令编号=" + int.Parse(textBox2.Text.Trim()) ;
                    MyMeans.ExecuteSql(MyMeans.SQLstr);

                    for (int i = 0; i < dataGridView4.Rows.Count; i++)
                    {

                        DataGridViewCheckBoxCell dgvCheckBoxCell = dataGridView4.Rows[i].Cells[0] as DataGridViewCheckBoxCell;//获取DataGridViewCheckBoxCell对象；

                        if (dgvCheckBoxCell.Value == "1")               //判断该DataGridViewCheckBoxCell是否选中；
                        {
                            SLchezhan += (dataGridView4.Rows[i].Cells[1].Value.ToString().Trim() + "#");
                            MyMeans.SQLstr = "Insert into 受令车站对应表(命令编号,受令单位)values(" + int.Parse(textBox2.Text.Trim()) + ",'" + dataGridView4.Rows[i].Cells[1].Value.ToString().Trim() + "')";
                            MyMeans.ExecuteSql(MyMeans.SQLstr);
                        }
                    }
                    DataSet ds = MyMeans.GetDataSet("Select*from daifaml");
                    dataGridView1.DataSource = ds.Tables[0];
                    textBox2.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    comboBox1.Text = "";
                    richTextBox1.Text = "";
                    for (int i = 0; i < dataGridView4.Rows.Count; i++)
                    {

                        dataGridView4.Rows[i].Cells[0].Value = "0";

                    }
                    isChongxinBj = 0; //重新编辑并保存成功之后，将isChongxinBj的值置为0；
                }
            }
            else
            {
                MessageBox.Show("请将命令补充完整");
            }

        }
        private void button5_Click(object sender, EventArgs e)          //“发送”按钮，点击发送调度命令，同时将调度命令保存在发令箱
        {
            int a = 0, b = 0, c = 0, d = 0, f = 0, g = 0,h=0;
            string SLchezhan = null;
            for (int i = 0; i < dataGridView4.Rows.Count; i++)
            {

                DataGridViewCheckBoxCell dgvCheckBoxCell = dataGridView4.Rows[i].Cells[0] as DataGridViewCheckBoxCell;//获取DataGridViewCheckBoxCell对象；

                if (dgvCheckBoxCell.Value == "1")               //判断该DataGridViewCheckBoxCell是否选中；
                {
                    SLchezhan += (dataGridView4.Rows[i].Cells[1].Value.ToString().Trim() + "#");
                   
                }
            }
            if (textBox2.Text.Trim() != "")
            {
                a = 1;
            }
            if (comboBox1.Text.Trim() != "")
            {
                b = 1;
            }
            if (textBox4.Text.Trim() != "")
            {
                c = 1;
            }
            if (textBox5.Text.Trim() != "")
            {
                d = 1;
            }
            if (dateTimePicker1.Text != "")
            {
                f = 1;
            }
            if (richTextBox1.Text.Trim() != "")
            {
                g = 1;
            }
            if (SLchezhan != "")
            {
                h = 1;
            }
            if (a == 1 & b == 1 & c == 1 & d == 1 & f == 1 & g == 1 & h == 1)
            {
                try
                {

                    byte[] sendByts = new byte[1024];
                    //发送调度命令给服务器
                    string sendStr = textBox2.Text.Trim() + "#" + comboBox1.Text.Trim() + "#" + textBox5.Text.Trim() + "#" + dateTimePicker1.Text + "#" + richTextBox1.Text.Trim();
                    sendByts = Encoding.UTF8.GetBytes(sendStr);
                    socket.Send(sendByts, sendByts.Length, 0);
                    //发送受令车站信息给服务；      
                    sendByts = Encoding.UTF8.GetBytes(SLchezhan);
                    socket.Send(sendByts, sendByts.Length, 0);
                    MessageBox.Show("命令发送成功");
                    // 将已发送的调度命令存储到发送命令列表
                    MyMeans.SQLstr = "Insert into fasongml(命令编号,命令类型,调度员姓名,发令单位,发令时间,命令内容,当前状态)values(" + int.Parse(textBox2.Text.Trim()) + ",'" + comboBox1.Text.Trim() + "','" + textBox4.Text.Trim() + "','" + textBox5.Text.Trim() + "','" + dateTimePicker1.Text.Trim() + "','" + richTextBox1.Text.Trim() + "','已发送')";// 注意圆括号中的 ， 用英文形式
                    MyMeans.ExecuteSql(MyMeans.SQLstr);
                    //将调度命令的受令车站信息加入到“受令车站对应表”中；
                    for (int i = 0; i < dataGridView4.Rows.Count; i++)
                    {

                        DataGridViewCheckBoxCell dgvCheckBoxCell = dataGridView4.Rows[i].Cells[0] as DataGridViewCheckBoxCell;//获取DataGridViewCheckBoxCell对象；

                        if (dgvCheckBoxCell.Value == "1")               //判断该DataGridViewCheckBoxCell是否选中；
                        {

                            MyMeans.SQLstr = "Insert into 受令车站对应表(命令编号,受令单位)values(" + int.Parse(textBox2.Text.Trim()) + ",'" + dataGridView4.Rows[i].Cells[1].Value.ToString().Trim() + "')";
                            MyMeans.ExecuteSql(MyMeans.SQLstr);
                        }
                    }
                    //更新界面显示；
                    DataSet ds;
                    ds = MyMeans.GetDataSet("Select*from fasongml");
                    dataGridView2.DataSource = ds.Tables[0];
                }
                catch
                {
                    MessageBox.Show("命令发送失败");
                    // 将调度命令存储到待发命令列表；
                    MyMeans.SQLstr = "Insert into daifaml(命令编号,命令类型,调度员姓名,发令单位,起草时间,命令内容,当前状态)values(" + int.Parse(textBox2.Text.Trim()) + ",'" + comboBox1.Text.Trim() + "','" + textBox4.Text.Trim() + "','" + textBox5.Text.Trim() + "','" + dateTimePicker1.Text.Trim() + "','" + richTextBox1.Text.Trim() + "','已发送')";// 注意圆括号中的 ， 用英文形式
                    MyMeans.ExecuteSql(MyMeans.SQLstr);
                    //将调度命令的受令车站信息加入到“受令车站对应表”中；
                    for (int i = 0; i < dataGridView4.Rows.Count; i++)
                    {

                        DataGridViewCheckBoxCell dgvCheckBoxCell = dataGridView4.Rows[i].Cells[0] as DataGridViewCheckBoxCell;//获取DataGridViewCheckBoxCell对象；

                        if (dgvCheckBoxCell.Value == "1")               //判断该DataGridViewCheckBoxCell是否选中；
                        {

                            MyMeans.SQLstr = "Insert into 受令车站对应表(命令编号,受令单位)values(" + int.Parse(textBox2.Text.Trim()) + ",'" + dataGridView4.Rows[i].Cells[1].Value.ToString().Trim() + "')";
                            MyMeans.ExecuteSql(MyMeans.SQLstr);
                        }
                    }
                    //更新界面显示；
                    DataSet ds;
                    ds = MyMeans.GetDataSet("Select*from daifaml");
                    dataGridView1.DataSource = ds.Tables[0];
                }
   
                textBox2.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                comboBox1.Text = "";
                richTextBox1.Text = "";
                for (int i = 0; i < dataGridView4.Rows.Count; i++)
                {

                    dataGridView4.Rows[i].Cells[0].Value = "0";

                }
            }
            else
            {
                MessageBox.Show("请将命令补充完整");
            }

        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)              //“连接服务器”按钮，点击按钮将连接服务器
        {
            if (label7.Text.Trim() == "通信断开")
            {

                try //连接服务器，若连接成功，通信状态显示为“通信正常”；
                {
                    //当socket调用close()方法后，关闭连接并释放连接占用的资源，所以当重新连接时，需给socket重新赋值
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(IPP);

                    string SendStr = "行调系统";
                    byte[] Bs = Encoding.UTF8.GetBytes(SendStr);
                    socket.Send(Bs, Bs.Length, 0);
                    label7.Text = "通信正常";
                    jieshouMsg = new Thread(new ThreadStart(RecMsg));
                    jieshouMsg.IsBackground = true;
                    jieshouMsg.Start();
                }
                catch
                {
                    MessageBox.Show("连接失败");       //若连接服务器失败，显示提示窗口“连接失败”；
                }
            }
            else
            {
                MessageBox.Show("已连接服务器");
            }
        }
        //接受车站发来的签收状态
        public void RecMsg()
        {
            int i=0;  //0表示某条调度命令所有受令车站均已签收，1表示存在受令车站没有签收；
            while (true)
            {
                byte[] Msg = new byte[1024];
                int Bytes=1;
                string receiveMsg = null;
                char[] fenge = { '#' };
                string[] msg;
                try
                {
                    Bytes = socket.Receive(Msg, Msg.Length, 0);
                    if(Bytes==0)
                    {
                        
                        label7.Text = "通信断开";
                        socket.Close();
                        //在调用Abort()方法的线程上引发ThreadAbortException异常，以开始终止线程。
                        //即调用此方法会自动引发一个异常，程序便运行到catch语句块
                        Thread.CurrentThread.Abort();
                    }
                }
                catch(Exception ex)
                {
                    //GetType()获取当前实例的运行类型
                    //typeof()活动类型声明的泛型类型
                    if (ex.GetType() == typeof(ThreadAbortException))    //如果捕捉到的异常为ThreadAbortException异常，则什么都不做
                    { }
                    else
                    {

                        label7.Text = "通信断开";
                        socket.Close();
                        Thread.CurrentThread.Abort();
                    }
           
                }
                
                receiveMsg = Encoding.UTF8.GetString(Msg, 0, Bytes);
                msg = receiveMsg.Split(fenge);
                if (msg[0] == "断开")    //如果服务器发来的是断开通信连接的信息，则终止线程；
                {

                    
                    label7.Text = "通信断开";
                    socket.Close();
                    Thread.CurrentThread.Abort(); //获取当前正在运行的线程，并将其终止；

                }
                else
                {
                    //根据命令编号和受令单位更新 调度命令的相应车站的签收状态
                    MyMeans.SQLstr = "Update 受令车站对应表 Set 签收状态='签收',签收人='" + msg[2] + "',签收时间='" + msg[3] + "',签收结果='" + msg[4] + "'where 命令编号=" + int.Parse(msg[0]) + "and 受令单位='" + msg[1] + "'";
                    MyMeans.ExecuteSql(MyMeans.SQLstr);
                    MyMeans.SQLstr = "select*from 受令车站对应表 where 命令编号=" + int.Parse(msg[0]);
                    SqlDataReader Sdr = MyMeans.GgtDataReader(MyMeans.SQLstr);
                    while (Sdr.Read())
                    {
                        //如果有受令车站没有签收，将i置为1；
                        if (Sdr[2].ToString().Trim() == "")
                        {
                            i = 1;
                        }
                    }
                    if (i == 0)  //i=0表示所有车站均已签收；
                    {
                        MyMeans.SQLstr = "select*from fasongml where 命令编号=" + int.Parse(msg[0]) ;
                        SqlDataReader read = MyMeans.GgtDataReader(MyMeans.SQLstr);
                        read.Read();
                        MyMeans.SQLstr = "Insert into yifaml(命令编号,命令类型,调度员姓名,发令单位,发令时间,命令内容,签收状态)values(" + int.Parse(read[0].ToString().Trim()) + ",'" + read[1].ToString().Trim() + "','" + read[2].ToString().Trim() + "','" + read[3].ToString().Trim() + "','" + read[4].ToString().Trim() + "','" + read[5].ToString().Trim() + "','全部签收')";
                        MyMeans.ExecuteSql(MyMeans.SQLstr);
                        MyMeans.SQLstr = "Delete from fasongml where 命令编号=" + int.Parse(msg[0]) ;
                        MyMeans.ExecuteSql(MyMeans.SQLstr);
                        
                    }
                    DataSet ds;
                 
                    ds = MyMeans.GetDataSet("Select*from fasongml");
                    dataGridView2.DataSource = ds.Tables[0];
                    MyMeans.Close();
                    ds = MyMeans.GetDataSet("Select*from yifaml");
                    dataGridView3.DataSource = ds.Tables[0];
                    MyMeans.Close();
                }

            }

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            //在关闭Form2时关闭整个应用程序,并给出提示框
            DialogResult dr = MessageBox.Show("关闭窗体后，程序将退出", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(dr==DialogResult.Yes)
            {
                e.Cancel = false;
                try
                {
                    string SendStr = "断开#行调系统";     //关闭窗体时发送车站名称给服务器，清除服务器字典中的数据，保证下次可以正常连接
                    byte[] Bs = Encoding.UTF8.GetBytes(SendStr);
                    socket.Send(Bs, Bs.Length, 0);
                    jieshouMsg.Abort();
                }
                catch
                { }

                Application.Exit(); //在关闭Form2时关闭整个应用程序
            }
            else
            {
                e.Cancel = true;
            }
           
           
        }
        private void button6_Click(object sender, EventArgs e)
        {    
            Number_ML = 6000;
            SqlDataReader Sdr;
            MyMeans.SQLstr = "select*from daifaml";
            Sdr= MyMeans.GgtDataReader(MyMeans.SQLstr);
            while (Sdr.Read())
            {
                if (int.Parse(Sdr[0].ToString().Trim()) > Number_ML)
                {
                    Number_ML = int.Parse(Sdr[0].ToString());
                }
            }
            MyMeans.SQLstr = "select*from fasongml";
            Sdr = MyMeans.GgtDataReader(MyMeans.SQLstr);
            while (Sdr.Read())
            {
                if (int.Parse(Sdr[0].ToString().Trim()) > Number_ML)
                {
                    Number_ML = int.Parse(Sdr[0].ToString());
                }
            }
            MyMeans.SQLstr = "select*from yifaml";
            Sdr = MyMeans.GgtDataReader(MyMeans.SQLstr);
            while (Sdr.Read())
            {
                if (int.Parse(Sdr[0].ToString().Trim()) > Number_ML)
                {
                    Number_ML = int.Parse(Sdr[0].ToString());
                }
            }
            Number_ML++;
            textBox2.Text = Number_ML.ToString();

            textBox4.Text = Form1.Uers_Now;
            textBox5.Text = "调度台";
            //加载模板内容
            MyMeans.linq = new linqtosqlClassDataContext(MyMeans.Constr);    //创建LINQ连接对象
            String StrLeiXing = comboBox1.Text;                              //获取选中的命令模板
            //根据选中的命令模板获取其详细信息  
            var result = from items in MyMeans.linq.mlmoban
                         where items.命令类型 == StrLeiXing
                         select new
                         {
                             Number = items.模板编号,
                             命令类型 = items.命令类型,
                             NeiRong = items.命令内容
                         };
            //在命令内容正文控件中显示选中命令模板的命令内容
            foreach (var item in result)
            {
                richTextBox1.Text = item.NeiRong.ToString();
            }
          
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        //待发命令列表的右键菜单；
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dataGridView1.ClearSelection();
                dataGridView1.Rows[e.RowIndex].Selected = true; //将鼠标位置所在行设置为选中状态；
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button4.Enabled = true;
            button5.Enabled = true;
            isChongxinBj = 1;
            textBox2.Text = dataGridView1.SelectedCells[0].Value.ToString();
            comboBox1.Text = dataGridView1.SelectedCells[1].Value.ToString();
            textBox4.Text = dataGridView1.SelectedCells[2].Value.ToString();
            textBox5.Text = dataGridView1.SelectedCells[3].Value.ToString();
            dateTimePicker1.Text = dataGridView1.SelectedCells[4].Value.ToString();
            richTextBox1.Text = dataGridView1.SelectedCells[5].Value.ToString();
            textBox3.Text = "未发送";
            string StrNumber = dataGridView1.SelectedCells[0].Value.ToString();
            DataSet ds;
            ds = MyMeans.GetDataSet("select*from shoulinglb");
            dataGridView4.DataSource = ds.Tables[0];

            for (int i = 0; i < (dataGridView4.Rows.Count - 1); i++)//受令列表中实际为6行，但只有5个受令车站，故i最大为5；
            {
                //注意每次遍历都需要重新从数据库中的读取受令单位，因为Read()方法是读取到SqlDataReader的下一条记录；
                MyMeans.SQLstr = "Select 受令单位,签收状态,签收人,签收时间 from 受令车站对应表 where 命令编号=" + StrNumber ;
                SqlDataReader Sds = MyMeans.GgtDataReader(MyMeans.SQLstr);
                while (Sds.Read())
                {
                    string a = dataGridView4.Rows[i].Cells[1].Value.ToString().Trim();
                    string b = Sds[0].ToString().Trim();
                    if (b == a)
                    {
                        DataGridViewCheckBoxCell dgvCheckBoxCell_1 = dataGridView4.Rows[i].Cells[0] as DataGridViewCheckBoxCell;
                        dgvCheckBoxCell_1.Value = 1;
                    }
                }
            }
        }
     
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SC_ml = dataGridView1.SelectedCells[0].Value.ToString().Trim();
            MyMeans.SQLstr = "Delete from daifaml where 命令编号=" + int.Parse(SC_ml);
            MyMeans.ExecuteSql(MyMeans.SQLstr);
            MyMeans.SQLstr="Select*from daifaml";
            DataSet ds = MyMeans.GetDataSet(MyMeans.SQLstr);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Hide();
        }

        private void 下达ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte[] sendByts = new byte[1024];
            //发送调度命令给服务器
            string sendStr = dataGridView1.SelectedCells[0].Value.ToString().Trim()+ "#" + dataGridView1.SelectedCells[1].Value.ToString().Trim()+"#" + dataGridView1.SelectedCells[3].Value.ToString().Trim()+ "#" + dateTimePicker1.Text.Trim()+ "#" + dataGridView1.SelectedCells[5].Value.ToString().Trim();
            sendByts = Encoding.UTF8.GetBytes(sendStr);
            socket.Send(sendByts, sendByts.Length, 0);
            //发送受令车站信息给服务器；
            string SC_ml = dataGridView1.SelectedCells[0].Value.ToString().Trim();
            MyMeans.SQLstr = "select*from 受令车站对应表 where 命令编号=" + int.Parse(SC_ml);
            SqlDataReader read = MyMeans.GgtDataReader(MyMeans.SQLstr);
            string SLchezhan=null;
            while (read.Read())
            {
                SLchezhan += (read[1].ToString().Trim() + "#");
            }
            sendByts = Encoding.UTF8.GetBytes(SLchezhan);
            socket.Send(sendByts, sendByts.Length, 0);
            MessageBox.Show("命令发送成功");

            DataSet ds;
            MyMeans.SQLstr = "Insert into fasongml(命令编号,命令类型,调度员姓名,发令单位,发令时间,命令内容,当前状态)values(" + int.Parse(SC_ml) + ",'" + dataGridView1.SelectedCells[1].Value.ToString().Trim() + "','" + dataGridView1.SelectedCells[2].Value.ToString().Trim() + "','" + dataGridView1.SelectedCells[3].Value.ToString().Trim() + "','" + dateTimePicker1.Text.Trim() + "','" + dataGridView1.SelectedCells[5].Value.ToString().Trim() + "','已发送')";// 注意圆括号中的 ， 用英文形式
            MyMeans.ExecuteSql(MyMeans.SQLstr);
            ds = MyMeans.GetDataSet("Select*from fasongml");
            dataGridView2.DataSource = ds.Tables[0];
            MyMeans.Close();

            
            MyMeans.SQLstr = "Delete from daifaml where 命令编号=" + int.Parse(SC_ml);
            MyMeans.ExecuteSql(MyMeans.SQLstr);
            MyMeans.SQLstr = "Select*from daifaml";
            ds = MyMeans.GetDataSet(MyMeans.SQLstr);
            dataGridView1.DataSource = ds.Tables[0];
 
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button4.Enabled = true;
            button5.Enabled = true;
            textBox2.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            comboBox1.Text = "";
            richTextBox1.Text = "";
            MyMeans.SQLstr="select*from shoulinglb";
            DataSet ds=MyMeans.GetDataSet(MyMeans.SQLstr);
            dataGridView4.DataSource=ds.Tables[0];
            for (int i = 0; i < dataGridView4.Rows.Count; i++)
            {

                dataGridView4.Rows[i].Cells[0].Value = "0";

            }
            DataSet dss;
            dss = MyMeans.GetDataSet("Select*from fasongml");
            dataGridView2.DataSource = dss.Tables[0];
            MyMeans.Close();
            dss = MyMeans.GetDataSet("Select*from yifaml");
            dataGridView3.DataSource = dss.Tables[0];
        }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            textBox2.Text = dataGridView2.SelectedCells[0].Value.ToString();
            comboBox1.Text = dataGridView2.SelectedCells[1].Value.ToString();
            textBox4.Text = dataGridView2.SelectedCells[2].Value.ToString();
            textBox5.Text = dataGridView2.SelectedCells[3].Value.ToString();
            dateTimePicker1.Text = dataGridView2.SelectedCells[4].Value.ToString();
            richTextBox1.Text = dataGridView2.SelectedCells[5].Value.ToString();
            textBox3.Text = "已发送";
            string StrNumber = dataGridView2.SelectedCells[0].Value.ToString();
            DataSet ds;
            ds = MyMeans.GetDataSet("select 受令单位,签收状态,签收人,签收时间,签收结果 from 受令车站对应表 where 命令编号='" + StrNumber + "'");
            dataGridView4.DataSource = ds.Tables[0];

            for (int i = 0; i < (dataGridView4.Rows.Count - 1); i++)//受令列表中实际为6行，但只有5个受令车站，故i最大为5；
            {
                //注意每次遍历都需要重新从数据库中的读取受令单位，因为Read()方法是读取到SqlDataReader的下一条记录；
                MyMeans.SQLstr = "Select 受令单位,签收状态,签收人,签收时间 from 受令车站对应表 where 命令编号='" + StrNumber + "'";
                SqlDataReader Sds = MyMeans.GgtDataReader(MyMeans.SQLstr);
                while (Sds.Read())
                {
                    string a = dataGridView4.Rows[i].Cells[1].Value.ToString().Trim();
                    string b = Sds[0].ToString().Trim();
                    if (b == a)
                    {
                        DataGridViewCheckBoxCell dgvCheckBoxCell_1 = dataGridView4.Rows[i].Cells[0] as DataGridViewCheckBoxCell;
                        dgvCheckBoxCell_1.Value = 1;
                    }
                }
            }
            button4.Enabled = false;
            button5.Enabled = false;
        }

        private void richTextBox1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void groupBox7_Enter_1(object sender, EventArgs e)
        {

        }
    }
 
}
