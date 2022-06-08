using System;
using System.Collections.Generic;
using System.Data;
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

