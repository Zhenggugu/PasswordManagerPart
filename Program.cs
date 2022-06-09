using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace PasswordManagerPart
{
    class Program
    {
        static void Main(string[] args)
        {

            Reuse reuse = new();
            reuse.Reusing();
            CheckComplex checkComplex = new();
            checkComplex.Check();
        }
    }
    public class CheckComplex
    {
        public bool Match(string password)//判断复杂度;T为强
        {
            Regex regex = new(@"^[a-zA-Z0-9_-]{6,20}$");//暂时不管，不知道为啥114514都是强
            if (regex.IsMatch(password))
            {
                return true;
            }
            else
                return false;
        }
        public void Check()//检测所有密码版
        {
            string conStr = "server=localhost;user=root;password=123456;database=software";
            MySqlConnection con = new MySqlConnection(conStr);
            con.Open();
            MySqlCommand command = new MySqlCommand();
            command.Connection = con;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "SELECT PASS FROM passw;";
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<string> passwd = new();
            while (mySqlDataReader.Read())
            {
                passwd.Add(mySqlDataReader.GetString(0));
            }
            mySqlDataReader.Close();
            //此处应有一个解密过程
            foreach (var item in passwd)
            {
                if(Match(item))
                    Console.WriteLine(item+"强度高");
                else
                    Console.WriteLine(item+"强度低，建议修改");
            }
        }
    }
    public class Reuse
    {

        public int Reusing()
        {
            int count = 0;
            string conStr = "server=localhost;user=root;password=123456;database=software";
            MySqlConnection con = new MySqlConnection(conStr);
            con.Open();
            MySqlCommand command = new MySqlCommand();
            command.Connection = con;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "SELECT DISTINCT PASS FROM passw;";
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<string> password = new();
            while(mySqlDataReader.Read())
            {
                password.Add(mySqlDataReader.GetString(0));
                //command.CommandText = "SELECT COUNT(*) FROM passw WHERE PASS=" + ;
                //MySqlDataReader reader1 = command.ExecuteReader();
                //count += reader1.GetInt32(0);
            }
            mySqlDataReader.Close();
            //此处应有一个解密过程
            foreach (var item in password)
            {
                command.CommandText = "SELECT COUNT(*) FROM passw WHERE PASS=" + item;
                MySqlDataReader reader1 = command.ExecuteReader();
                reader1.Read();
                count += reader1.GetInt32(0);
                reader1.Close();
            }
            Console.WriteLine("重用密码数量"+count);
            con.Close();
            return 0;
        }
    }
}

