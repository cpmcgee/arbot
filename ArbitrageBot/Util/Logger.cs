using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ArbitrageBot.Util
{
    public static class Logger
    {
        public static StreamWriter sw;

        public static int ct = 0;

        public static void Initialize()
        {
            string fileName = DateTime.Today.ToString();
            while (File.Exists(fileName))
            {
                fileName = DateTime.Today.ToString() + "_" + ++ct;
            }
            sw = new StreamWriter(Config.GetLogFilePath() + fileName + ".txt");
        }
        
        public static void INFO(string msg)
        {
            msg = "[INFO] " + msg;
            sw.WriteLine(msg);
            Console.WriteLine(msg);
        }

        public static void ERROR(string msg)
        {
            msg = "[ERROR] " + msg;
            sw.WriteLine(msg);
            Console.WriteLine(msg);
        }
    }
}
