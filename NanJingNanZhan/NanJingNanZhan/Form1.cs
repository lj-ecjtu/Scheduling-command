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
using System.Threading;
using System.Drawing.Drawing2D;

namespace NanJingNanZhan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        

        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static int port = 2000;
        static string host = "192.168.1.102";
        static IPAddress ip = IPAddress.Parse(host);
        IPEndPoint IPP = new IPEndPoint(ip, port);
        Thread RecMsg=null;

        

        DataTable table;
       
        private void Form2_Load(object sender, EventArgs e)
        {
            
           
            Control.CheckForIllegalCrossThreadCalls = false;
            try
            {
                socket.Connect(IPP);
                //将车务终端名称发送给服务器；
                string SendStr = "南京南站";
                byte[] Bs = Encoding.UTF8.GetBytes(SendStr);
                socket.Send(Bs, Bs.Length, 0);
                label7.Text = "通信正常";
                //新建接收命令的线程
                RecMsg = new Thread(new ParameterizedThreadStart(ReciveMsg));
                RecMsg.IsBackground = true;                        //将线程设置为后台线程，此时程序控制权归主线程；
                RecMsg.Start(socket);

            }
            catch
            { }

            try
            {
                //加载调度命令
                DataSet dataset = MyMeans.GetDataSet("Select*from qianshouml");
                table = dataset.Tables[0];


                //检查是否所有调度命令都有签收
                MyMeans.SQLstr = "select*from qianshouml ";
                SqlDataReader Sdr = MyMeans.GgtDataReader(MyMeans.SQLstr);
                int i = 0;
                while (Sdr.Read())
                {
                    //如果有调度命令没有签收，将i置为1；
                    string a = Sdr[6].ToString().Trim();
                    if (a == "")
                    {
                        i = 1;
                    }
                }
                if (i == 1)  //i=0表示所有调度命令均已签收，则将指示按钮颜色设置为白色；
                {
                    button4.BackColor = Color.Red;

                }
            }
            catch
            { }
        }

       
        private void button2_Click(object sender, EventArgs e)//“签收”按钮，点击按钮进行调度命令的签收；
        {
            try
            {


                if (dataGridView1.SelectedCells[5].Value.ToString().Trim() == "")
                {
                    
                    string sleNumber = dataGridView1.SelectedCells[0].Value.ToString().Trim();
                    
                    //发送签收命令；
                    byte[] Bs = new byte[1024];
                    Bs = Encoding.UTF8.GetBytes(sleNumber + "#南京南站#" + textBox3.Text + "#" + dateTimePicker2.Text + "#接收");
                    socket.Send(Bs, Bs.Length, 0);
                    Bs = Encoding.UTF8.GetBytes("行调系统");
                    socket.Send(Bs, Bs.Length, 0);

                    //将签收信息更新到数据库中；
                    MyMeans.SQLstr = "Update qianshouml set 是否签收='已签收',签收人='" + textBox3.Text + "',签收时间='" + dateTimePicker2.Text + "',签收结果='接受' where 命令编号=" + int.Parse(sleNumber);
                    MyMeans.ExecuteSql(MyMeans.SQLstr);
                    //在签收之后更新列表的内容；
                    DataSet dataset = MyMeans.GetDataSet("Select*from qianshouml");
                    table = dataset.Tables[0];

                    //检查是否所有调度命令都有签收
                    MyMeans.SQLstr = "select*from qianshouml ";
                    SqlDataReader Sdr = MyMeans.GgtDataReader(MyMeans.SQLstr);
                    int i = 0;
                    while (Sdr.Read())
                    {
                        //如果有调度命令没有签收，将i置为1；
                        string a = Sdr[6].ToString().Trim();
                        if (a == "")
                        {
                            i = 1;
                        }
                    }
                    if (i == 0)  //i=0表示所有调度命令均已签收，则将指示按钮颜色设置为白色；
                    {
                        button4.BackColor = Color.White;

                    }
                }
                else
                {
                    MessageBox.Show("该条命令已签收");
                }
            }
            catch
            {
                MessageBox.Show("签收失败");
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (label7.Text.Trim() == "通信断开")
            {
                try
                {
                    //当socket调用close()方法后，关闭连接并释放连接占用的资源，所以当重新连接时，需给socket重新赋值
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(IPP);
                    //将车务终端名称发送给服务器；
                    string SendStr = "南京南站";
                    byte[] Bs = Encoding.UTF8.GetBytes(SendStr);
                    socket.Send(Bs, Bs.Length, 0);
                    label7.Text = "通信正常";
                    //新建接收命令的线程
                    RecMsg = new Thread(new ParameterizedThreadStart(ReciveMsg));
                    RecMsg.IsBackground = true;                        //将线程设置为后台线程，此时程序控制权归主线程；
                    RecMsg.Start(socket);

                }
                catch
                {
                    MessageBox.Show("连接失败");
                }
            }
            else
            {
                MessageBox.Show("已连接服务器");
            }
        }
             //接受调度命令的方法；
            public  void ReciveMsg(object ClintSocket)
         {
            Socket sock=ClintSocket as Socket;
            while (true)
            {
                //接受行调系统发来的调度命令；
                string recvStr = "";
                byte[] recvBytes = new byte[1024];
                int bytes=1;
                char[] cha = { '#' };
                try
                {
                    bytes = sock.Receive(recvBytes, recvBytes.Length, 0);
                    if(bytes==0)
                    {
                        label7.Text = "通信断开";
                        sock.Close();
                        Thread.CurrentThread.Abort();
                    }
                }
                catch(Exception ex)
                {
                    if (ex.GetType() == typeof(ThreadAbortException))
                    { }
                    else
                    {
                        label7.Text = "通信断开";
                        sock.Close();
                        Thread.CurrentThread.Abort();
                    }
                }
                recvStr = Encoding.UTF8.GetString(recvBytes, 0, bytes);
                string[] RecMsg = recvStr.Split(cha);
                //将新接受到的命令加入到数据库
                if (RecMsg[0] == "断开")
                {

                    sock.Close();
                    label7.Text = "通信断开";
                    Thread.CurrentThread.Abort();
                }
                else
                {
                    MyMeans.SQLstr = "Insert into qianshouml(命令编号,命令类型,发令单位,发令时间,命令内容)values(" + int.Parse(RecMsg[0]) + ",'" + RecMsg[1] + "','" + RecMsg[2] + "','" + RecMsg[3] + "','" + RecMsg[4] + "')";// 注意圆括号中的 ， 用英文形式
                    MyMeans.My_conn = new SqlConnection(MyMeans.Constr);
                    MyMeans.My_conn.Open();
                    SqlCommand Com = new SqlCommand();
                    Com.Connection = MyMeans.My_conn;
                    Com.CommandText = MyMeans.SQLstr;
                    Com.CommandType = CommandType.Text;
                    Com.ExecuteNonQuery();
                    MyMeans.My_conn.Dispose();
                    //将接受消息后的数据表重新查询并绑定到列表上
                    DataSet dataset = MyMeans.GetDataSet("Select*from qianshouml");
                    table = dataset.Tables[0];
                   
 
                    button4.BackColor = Color.Red;
                }
                
            }
         }
            //关闭窗体时断开socket连接，并终止线程（否则，调试程序将仍处于运行状态）
            private void Form2_FormClosing(object sender, FormClosingEventArgs e)
            {
                try
                {
                    string SendStr = "断开#南京南站";     //关闭窗体时发送车站名称给服务器，清除服务器字典中的数据，保证下次可以正常连接
                    byte[] Bs = Encoding.UTF8.GetBytes(SendStr);
                    socket.Send(Bs, Bs.Length, 0);
                    socket.Close();
                    RecMsg.Abort();
                }
                catch
                { }
            }

            private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
            {
                textBox1.Text = dataGridView1.SelectedCells[0].Value.ToString();
                textBox4.Text = dataGridView1.SelectedCells[1].Value.ToString();
                textBox2.Text = dataGridView1.SelectedCells[2].Value.ToString();
                textBox5.Text = dataGridView1.SelectedCells[3].Value.ToString();
                richTextBox1.Text = dataGridView1.SelectedCells[4].Value.ToString();

         
               
             }
            //使跨线程调用控件不出错；
            private void timer1_Tick(object sender, EventArgs e)
            {
                dataGridView1.DataSource = table;
            }

        private void button3_Click(object sender, EventArgs e)
       {
             try
             {

                  if(dataGridView1.SelectedCells[5].Value.ToString().Trim() == "")
                  {
                     
                      string sleNumber = dataGridView1.SelectedCells[0].Value.ToString().Trim();

                      //发送签收命令；
                      byte[] Bs = new byte[1024];
                      Bs = Encoding.UTF8.GetBytes(sleNumber + "#南京南站#" + textBox3.Text + "#" + dateTimePicker2.Text + "#拒绝");
                      socket.Send(Bs, Bs.Length, 0);
                      Bs = Encoding.UTF8.GetBytes("行调系统");
                      socket.Send(Bs, Bs.Length, 0);

                      //将签收信息更新到数据库中；
                      MyMeans.SQLstr = "Update qianshouml set 是否签收='已签收',签收人='" + textBox3.Text + "',签收时间='" + dateTimePicker2.Text + "',签收结果='拒绝' where 命令编号=" + int.Parse(sleNumber);//每个“列=值”对之间用逗号隔开
                      MyMeans.ExecuteSql(MyMeans.SQLstr);

                      //在签收之后更新列表的内容；
                      DataSet dataset = MyMeans.GetDataSet("Select*from qianshouml");
                      table = dataset.Tables[0];

                      //检查是否所有调度命令都有签收
                      MyMeans.SQLstr = "select*from qianshouml ";
                      SqlDataReader Sdr = MyMeans.GgtDataReader(MyMeans.SQLstr);

                      int i = 0;
                      while (Sdr.Read())
                      {
                          //如果有调度命令没有签收，将i置为1；
                          string a = Sdr[6].ToString().Trim();
                         if (a == "")
                         {
                            i = 1;
                         }
                      }
                      if (i == 0)  //i=0表示所有调度命令均已签收；
                      {
                        button4.BackColor = Color.White;

                      }

                  }
                  else
                  {
                      MessageBox.Show("该条命令已签收");

                  }
             }
             catch
             {
                 MessageBox.Show("签收失败");
             }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //检查是否所有调度命令都有签收
            MyMeans.SQLstr = "select*from qianshouml ";
            SqlDataReader Sdr = MyMeans.GgtDataReader(MyMeans.SQLstr);
            int i = 0;
            while (Sdr.Read())
            {
                //如果有调度命令没有签收，将i置为1；
                string a = Sdr[6].ToString().Trim();
                if (a == "")
                {
                    i = 1;
                }
            }
            if (i == 0)  //i=0表示所有调度命令均已签收，则将指示按钮颜色设置为白色；
            {
                button4.BackColor = Color.White;

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox4.Text = "";
            textBox2.Text = "";
            textBox5.Text = "";
            richTextBox1.Text = "";
            
  
        }
    }
}
