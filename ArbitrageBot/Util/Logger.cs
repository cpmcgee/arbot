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
        private static int level;

        /// <summary>
        /// logger levels:
        /// 1 - least verbose
        /// 5 - most verbose
        /// </summary>
        /// <param name="level"></param>
        public static void Initialize(int verbosityLevel)
        {
            level = verbosityLevel;
            string path = Config.GetLogFilePath();
            string fileName = String.Format("{0:MM/dd/yyyy}", DateTime.Now).Replace("/", "-").Replace(":", ".") + ".txt";
            while (File.Exists(path + fileName))
            {
                fileName = String.Format("{0:MM/dd/yyyy}", DateTime.Now).Replace("/", "-").Replace(":", ".") + "_" + ++ct + ".txt";
            }
            fileName = path + fileName;
            sw = new StreamWriter(fileName);
        }
        
        public static void INFO(string msg, int lvl)
        {
            if (lvl <= level)
            {
                msg = "[INFO] " + msg;
                sw.WriteLine(msg);
                Console.WriteLine(msg);
            }
        }

        public static void ERROR(string msg, int lvl)
        {
            if (lvl <= level)
            {
                msg = "[ERROR] " + msg;
                sw.WriteLine(msg);
                Console.WriteLine(msg);
            }
        }

        public static void WRITE(string msg)
        {
            sw.WriteLine(msg);
            Console.WriteLine(msg);
        }

        public static void BREAK()
        {
            sw.WriteLine("\n");
            Console.WriteLine("\n");
        }

        public static void Close()
        {
            sw.Dispose();
        }
    }
}
