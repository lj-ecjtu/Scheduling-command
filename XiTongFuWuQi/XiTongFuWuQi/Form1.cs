using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Data.SqlClient;
namespace XiTongFuWuQi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Socket ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Socket ClintSocket = null;
        Socket SendSocket=null;              
        Dictionary<string,Socket> SocketList=new Dictionary<string,Socket>{};
        Dictionary<string, string> NameList = new Dictionary<string, string> {};
        string ipp = null;


        //启动服务器，开始监听客户端请求
        private void Form1_Load(object sender, EventArgs e)  //启动服务器，开始监听客户端请求
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            int port = 2000;
            string host = "192.168.43.40";
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint IPP = new IPEndPoint(ip, port);
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.Bind(IPP);
            ServerSocket.Listen(0);                         //0表示连接数量不限；
            
            Thread ThreadWatch = new Thread(new ThreadStart(WatchConnection));          //创建一个负责监听客户端的线程
            ThreadWatch.Start();                                                        //线程开始运行
            ThreadWatch.IsBackground = true;
            
        }
     

       
        //负责监听客户端请求的方法
         public  void WatchConnection()
        {
            
            //持续不断监听客户端发来的请求
            while (true) 
          {
              ClintSocket = ServerSocket.Accept();                                   //接受客户端请求
              ipp = ClintSocket.RemoteEndPoint.ToString().Trim();

              //将客户端IPEndPoint和对象socket存入dictionary;   
              SocketList.Add(ipp,ClintSocket);
                
              //接收客户端名称
              byte[] recvBytes = new byte[1024];  //字节型数组，1KB=1024B
              int bytes = ClintSocket.Receive(recvBytes, recvBytes.Length, 0);
              string recvStr = Encoding.UTF8.GetString(recvBytes, 0, bytes);

              //将客户端名和IPEndPoint存入dictionary；
              NameList.Add(recvStr, ipp); 
              
              //将客户端信息打印到界面 
              listBox1.Items.Add(recvStr+"    连接成功");
              listBox1.Items.Add("IPEndPoint: " + ipp);
                listBox1.Items.Add("\n");

              recvBytes.Initialize();
              Thread ClintThread = new Thread(new ParameterizedThreadStart(ChatMsg));  //对每一个请求客户端，创建一个通信线程，各个线程独立运行
              ClintThread.IsBackground = true;
              ClintThread.Start(recvStr);                                        //线程方法参数为客户端套接字对象
           
          }
       }
         //负责与客户端进行信息交互的方法，参数为客户端套接字对象
         public void ChatMsg(object name)
         {
            string clientname = name as string;
            ipp = NameList[clientname];
            Socket sock = SocketList[ipp];//sock 为当前连接的客户端

            
            //接受行调发来的命令,通过循环判断，发给受令车站
            while (true)
             {
                 string recvStr = "";
                 string recvCZ = null;
                 byte[] recvBytes = new byte[1024];
                 char[] split = { '#' };       //分割行调发来的命令为子字符串
                 int bytes=1;
                 try
                 {
                    bytes = sock.Receive(recvBytes, recvBytes.Length, 0);
                    //如果bytes==0，则为客户端断开连接
                    if (bytes==0)
                    {
                        //将该客户端信息从字典中清除
                        NameList.Remove(clientname);
                        SocketList.Remove(ipp);

                        listBox1.Items.Add(clientname + "    断开连接");
                        listBox1.Items.Add("IPEndPoint: " + ipp );
                        listBox1.Items.Add("\n");

                        sock.Close();

                        //在调用Abort()方法的线程上引发ThreadAbortException异常，以开始终止线程。
                        //即调用此方法会自动引发一个异常，程序便运行到catch语句块
                        Thread.CurrentThread.Abort(); 
                    }
                 }
                catch(Exception ex)
                {
                    //如果捕捉到的异常为ThreadAbortException异常，则什么都不做
                    if (ex.GetType() == typeof(ThreadAbortException))
                    { }
                    else
                    {
                        NameList.Remove(clientname);
                        SocketList.Remove(ipp);

                        listBox1.Items.Add(clientname + "    断开连接");
                        listBox1.Items.Add("IPEndPoint: " + ipp );
                        listBox1.Items.Add("\n");



                        sock.Close();
                        Thread.CurrentThread.Abort(); //获取当前正在运行的线程，并将其终止；
                    }
                }
                 recvStr = Encoding.UTF8.GetString(recvBytes, 0, bytes);

                 string[] RecMsg = recvStr.Split(split);
                 if (RecMsg[0] == "断开")    //如果客户端发来的是断开通信连接的信息，则将车站的信息在字典中清除
                 {
                     
                     NameList.Remove(clientname);
                     SocketList.Remove(ipp);

                     listBox1.Items.Add(clientname + "    断开连接");
                     listBox1.Items.Add("IPEndPoint: " + ipp );
                     listBox1.Items.Add("\n");



                    sock.Close();
                     Thread.CurrentThread.Abort(); //获取当前正在运行的线程，并将其终止；
                     
                 }
                 else
                 {

                     bytes = sock.Receive(recvBytes, recvBytes.Length, 0);
                     recvCZ = Encoding.UTF8.GetString(recvBytes, 0, bytes);
             
                      //分割行调发来的命令为子字符串
                     string[] RecCZ = recvCZ.Split(split);
                     Dictionary<string, string>.KeyCollection keyCol = NameList.Keys;
                     foreach (string CZ in RecCZ)
                     {
                         foreach (string key in keyCol)             //可通过逻辑“或”增加对其他受令车站的判断)                    //判断选择受令车站
                         {
                             if (CZ == key)
                             {
                                 SendSocket = SocketList[NameList[key]];        //找到受令车站对应的Socket;
                                 recvBytes = Encoding.UTF8.GetBytes(recvStr);
                                 SendSocket.Send(recvBytes, recvBytes.Length, 0);    //发送行调系统发出的调度命令
                               
                             }
                         }

                     }
                 }
             }
         }
   
   

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                string SendStr = "断开";     
                byte[] Bs = Encoding.UTF8.GetBytes(SendStr);  
                Dictionary<string, Socket>.ValueCollection value = SocketList.Values;
                foreach (Socket soc in value)
                {
                    soc.Send(Bs, Bs.Length, 0);
                   
                }

            }
            catch
            { }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
