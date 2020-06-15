using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace NanJingNanZhan
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
       static class MyMeans
     {
        public static SqlConnection My_conn;
        public static string SQLstr;
        public static string Constr = "server=.\\SQLEXPRESS;uid=sa;database=db_NanJingNanZhan;pwd=159753";
        
       public static DataSet GetDataSet(string SQLstr)         //方法五：利用Fill填充DataSet，并返回DataSet对象
        {
            My_conn = new SqlConnection(Constr);
            My_conn.Open();
            SqlCommand My_com = new SqlCommand();
            My_com.Connection = My_conn;
            My_com.CommandText = SQLstr;
            My_com.CommandType = CommandType.Text;
            SqlDataAdapter SQLda = new SqlDataAdapter(SQLstr, My_conn);
            DataSet My_DataSet = new DataSet();
            SQLda.Fill(My_DataSet);
            My_conn.Dispose();
            return My_DataSet;
           }
       public static void ExecuteSql(string SQLstr)              //方法四：通过Sqlcommand对象执行对数据库的添加、删除、修改操作
       {
           My_conn = new SqlConnection(Constr);
           My_conn.Open();
           SqlCommand SQLcom = new SqlCommand();
           SQLcom.Connection = My_conn;
           SQLcom.CommandText = SQLstr;
           SQLcom.CommandType = CommandType.Text;
           SQLcom.ExecuteNonQuery();
           SQLcom.Dispose();
           My_conn.Dispose();
       }
        public static SqlDataReader GgtDataReader(string SQLstr)      //方法三：以只读的方式读取数据库中的信息，返回SqlDataReader对象
        {
            My_conn = new SqlConnection(Constr);
            My_conn.Open();
            SqlCommand My_com = new SqlCommand();
            My_com.Connection = My_conn;
            My_com.CommandText = SQLstr;
            My_com.CommandType = CommandType.Text;
            SqlDataReader My_read = My_com.ExecuteReader();
            return My_read;
        }
    }
}
