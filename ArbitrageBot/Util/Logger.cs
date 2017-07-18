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
            string path = Config.GetLogFilePath();
            string fileName = DateTime.Today.ToString().Replace("/", "_").Replace(":", ".") + ".txt";
            while (File.Exists(path + fileName))
            {
                fileName = DateTime.Today.ToString().Replace("/", "_").Replace(":", ".") + "_" + ++ct + ".txt";
            }
            fileName = path + fileName;
            sw = new StreamWriter(fileName);
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

        public static void Close()
        {
            sw.Dispose();
        }
    }
}
