using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());       
         
        }
    }
    class MyMeans
    {
        public static SqlConnection My_conn;
        public static string SQLstr;
        public static linqtosqlClassDataContext linq;              //什么LINQ连接对象
        public static string Constr = "server=.\\SQLEXPRESS;uid=sa;database=db_data;pwd=159753";       
        public static SqlConnection Getcon()                    //方法一：用SqlConnection对象建立与数据库的链接
        {        
            My_conn = new SqlConnection(Constr);
            My_conn.Open();
            return My_conn;
        }
        public static void Close()                              //方法二：判断是否与数据库连接
        {
            if (My_conn.State == ConnectionState.Open)
            {
                My_conn.Close();
                My_conn.Dispose();
            }
        }
        public static SqlDataReader GgtDataReader(string SQLstr)      //方法三：以只读的方式读取数据库中的信息，返回SqlDataReader对象
        {
            Getcon();
            SqlCommand My_com = new SqlCommand();
            My_com.Connection = My_conn;
            My_com.CommandText = SQLstr;
            My_com.CommandType = CommandType.Text;
            SqlDataReader My_read = My_com.ExecuteReader();
            return My_read;
        }
        public static void ExecuteSql(string SQLstr)              //方法四：通过Sqlcommand对象执行对数据库的添加、删除、修改操作
        {
            Getcon();
            SqlCommand SQLcom = new SqlCommand();
            SQLcom.Connection = My_conn;
            SQLcom.CommandText = SQLstr;
            SQLcom.CommandType = CommandType.Text;
            SQLcom.ExecuteNonQuery();
            SQLcom.Dispose();
            Close();
        }
        public static DataSet GetDataSet(string SQLstr)   //方法五：利用Fill填充DataSet，并返回DataSet对象
        {
            Getcon();
            SqlCommand My_com = new SqlCommand();
            My_com.Connection = My_conn;
            My_com.CommandText = SQLstr;
            My_com.CommandType = CommandType.Text;
            SqlDataAdapter SQLda = new SqlDataAdapter(SQLstr, My_conn);
            DataSet My_DataSet = new DataSet();
            SQLda.Fill(My_DataSet);
            Close();
            return My_DataSet;
          
        }
    
         
     
            


        
       
    }
}
