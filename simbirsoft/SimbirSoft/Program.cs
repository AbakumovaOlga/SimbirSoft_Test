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

        static void Main(string[] args)
        {
            Console.Write("Введите url: ");
            string fullPath = Console.ReadLine();
            //string fullPath = "https://www.simbirsoft.com/";
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
                    foreach (Statistics stat in array)
                    {
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
