using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace SimbirSoft
{
    class Program
    {
        static HttpWebRequest req;
        static HttpWebResponse resp;
        static StreamReader sr;
        static string content;
        private static SQLiteConnection sql_con;
        private static SQLiteCommand sql_cmd;
        private static string sPath = "SS.db";


        static void Main(string[] args)
        {
            string fullPath = "https://www.simbirsoft.com/";
            try
            {
                req = (HttpWebRequest)WebRequest.Create(fullPath);
                resp = (HttpWebResponse)req.GetResponse();
                using (sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
                {
                    content = sr.ReadToEnd();


                    content = Regex.Replace(content, @"<script[^>]*>[\s\S]*?</script>", string.Empty);
                    content = Regex.Replace(content, "<style>[^>]+</style>", string.Empty);
                    content = Regex.Replace(content, "<[^>]+>", string.Empty);

                    string[] split = content.Split(new Char[] { ' ', ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '-', '\n', '\r', '\t' });

                    List<Statistics> array = new List<Statistics>();

                    foreach (string s in split)
                    {
                        bool ex = false;
                        foreach (Statistics stat in array)
                        {
                            if (s.Equals(stat.GetWord()))
                            {
                                stat.AddCount();
                                ex = true;
                                break;
                            }
                        }
                        if (!ex)
                        {
                            array.Add(new Statistics(s));
                        }
                    }
                    string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
                    foreach (Statistics stat in array)
                    {
                        string txtSQLQuery = "insert into Statistics (Word, Count) values ('" + stat.GetWord() + "', " + stat.GetCount() + ")";
                        sql_con = new SQLiteConnection("Data Source=" + sPath +";Version=3;New=False;Compress=True;");
                        sql_con.Open();
                        sql_cmd = sql_con.CreateCommand();
                        sql_cmd.CommandText = txtSQLQuery;
                        sql_cmd.ExecuteNonQuery();
                        sql_con.Close();

                        Console.WriteLine(stat.Print());
                    }
                }
			 Console.ReadLine();
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
        }
        public static void Log(string message)
        {
            File.AppendAllText("log.txt", message);
        }
    }
}
