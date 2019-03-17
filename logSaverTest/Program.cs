using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;


namespace logSaver
{
    public class logList
    {
        public string Date { get; set; }
        public string Status { get; set; }
        public string Event { get; set; }
        public string MessageV1 { get; set; }
        public string MessageV2 { get; set; }
    }
    public class Process
    {
        public List<logList> ReadLog(string link)
        {
            string[] dataString = File.ReadAllLines(link); // читаем построчно
            List<logList> dataList = new List<logList>();


            for (int i = 0; i < dataString.Length; i++)
            {
                Match m = Regex.Match(dataString[i], @"^([^|]*)(?:\|([^|]*))+"); // регулярка для разбиение данных, хотя в данном случае, думаю бы больше подошел бы Split
                if (m.Groups["1"].Value != "" && m.Groups["2"].Captures[0].Value != "" && m.Groups["2"].Captures[2].Value != "") // если в строке все верно заполнено
                {
                    string[] array = {
                    m.Groups["1"].Value,
                    m.Groups["2"].Captures[0].Value,
                    m.Groups["2"].Captures[1].Value,
                    m.Groups["2"].Captures[2].Value,
                    m.Groups["2"].Value
                                 };
                    dataList.Add(
                        new logList
                        {
                            Date = array[0],
                            Status = array[1],
                            Event = array[2],
                            MessageV1 = array[3],
                            MessageV2 = array[4]
                        }
                   );
                }
                else
                {
                    dataList[dataList.Count - 1].MessageV2 = dataList[dataList.Count - 1].MessageV2 + dataString[i]; // если строка не сожержит нужной нам маски, то продплжаем предыдущее сообщение
                    continue;
                }
            }
            return dataList;
        }
        public static bool InsertDB(List<logList> dataList)
        {
            try
            {
                foreach (var dataString in dataList)
                {
                    string connectionString = "server=localhost;user=root;database=library;password=12345;";
                    MySqlConnection connection = new MySqlConnection(connectionString);
                    connection.Open();
                    string query = "INSERT INTO logtable (Date, Status, Event, MessageV1, MessageV2) VALUES (@Date, @Status, @Event, @MessageV1, @MessageV2)";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Date", dataString.Date);
                    command.Parameters.AddWithValue("@Status", dataString.Status);
                    command.Parameters.AddWithValue("@Event", dataString.Event);
                    command.Parameters.AddWithValue("@MessageV1", dataString.MessageV1);
                    command.Parameters.AddWithValue("@MessageV2", dataString.MessageV2);
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Process proc = new Process();
            var dataList = proc.ReadLog(@"C:\Users\Александр\Desktop\TestTest.log");
            bool result = Process.InsertDB(dataList); // запись в бд, в моем случае mysql

            if (result == true) Console.Write("Все ок"); else Console.Write("Все не ок");

            Console.ReadLine();
        }
    }
}
