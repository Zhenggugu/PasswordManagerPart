using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace PasswordManagerPart
{

    class Program
    {
        static void Main(string[] args)
        {

            Reuse reuse = new();
            reuse.Reusing();
            //CheckComplex checkComplex = new();
            //checkComplex.Check();
            RandomPass randomPass = new();
            randomPass.MakePassword(10);
            randomPass.showPass();
            randomPass.InsertPass("1241", "hyper", "tiansuo", "132");
        }
    }
    class RandomPass
    {
        public string targetPassword = "";
        public string MakePassword(int length)//输入密码需要的长度
        {
            int RandNum;
            string randomchars = "abcdefghijkl*/-+.mnopqrstuvwxyz0123456789AB/*-+.CDEF@#$%^&*GHIJKL/*-+.MNOPQRSTUVWXYZ";
            Random random = new();
            for (int i = 0; i < length; i++)
            {
                RandNum = random.Next(randomchars.Length);
                targetPassword += randomchars[RandNum];
            }
            return targetPassword;
        }
        public void showPass()
        {
            if (targetPassword.Length == 0)
                Console.WriteLine("请先生成一个再显示行不");
            else
                Console.WriteLine("随机生成的密码是" + targetPassword + "不满意可以继续，满意建议保存到数据库");
        }

        public void InsertPass(string id, string description, string account_name, string user_id)//传入准备插入数据库需要的东西
        {
            if (targetPassword.Length == 0)
            {
                Console.WriteLine("拜托先生成个密码");
                return;
            }
            string conStr = "server=localhost;user=root;password=123456;database=pass_assistant";
            MySqlConnection con = new MySqlConnection(conStr);
            con.Open();
            MySqlCommand command = new MySqlCommand();
            command.Connection = con;
            command.CommandType = System.Data.CommandType.Text;
            //command.CommandText = @"INSERT INTO account_info VALUES(" + id + "," + account_name + "," +targetPassword+"," +user_id+");";
            command.CommandText = "INSERT INTO account_info VALUES(" + '"' + id + '"' + "," + '"' + account_name + '"' + "," + '"' + targetPassword + '"' + "," + '"' + user_id + '"' + ");";
            @command.CommandText.Replace("\\", "");
            Console.WriteLine(@command.CommandText);
            //command.ExecuteNonQuery();去不掉字符串里面的\放弃了
            con.Close();
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
            string conStr = "server=localhost;user=root;password=123456;database=pass_assistant";
            MySqlConnection con = new MySqlConnection(conStr);
            con.Open();
            MySqlCommand command = new MySqlCommand();
            command.Connection = con;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "SELECT account_password FROM account_info;";
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
                if (Match(item))
                    Console.WriteLine(item + "强度高");
                else
                    Console.WriteLine(item + "强度低，建议修改");
            }
            con.Close();
        }
    }
    public class Reuse
    {

        public int Reusing()
        {
            int count = 0;
            string conStr = "server=localhost;user=root;password=123456;database=pass_assistant";
            MySqlConnection con = new MySqlConnection(conStr);
            con.Open();
            MySqlCommand command = new MySqlCommand();
            command.Connection = con;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "SELECT DISTINCT account_password FROM account_info;";
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<string> password = new();
            while (mySqlDataReader.Read())
            {
                password.Add(mySqlDataReader.GetString(0));
            }
            mySqlDataReader.Close();
            //此处应有一个解密过程
            foreach (var item in password)
            {
                command.CommandText = $"SELECT COUNT(*) FROM account_info WHERE account_password='{item}';";
                MySqlDataReader reader1 = command.ExecuteReader();
                reader1.Read();
                count += reader1.GetInt32(0);
                reader1.Close();
            }
            Console.WriteLine("重用密码数量" + count);
            con.Close();
            return 0;
        }
    }
}

