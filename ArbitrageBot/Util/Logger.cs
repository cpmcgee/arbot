using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ArbitrageBot.Util
{
    [Flags]
    public enum LogLevel
    {
        None = 0,                 //        0
        Info = 1,                 //        1
        Debug = 2,                //       10
        Warning = 4,              //      100
        Error = 8,                //     1000
        FunctionalMessage = 16,   //    10000
        FunctionalError = 32,     //   100000
        All = 63                  //   111111
    }

    public static class Logger
    {
        public static StreamWriter sw;
        private static AbstractLogger logger;
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
            logger = new FileLogger(LogLevel.All);
            ConsoleLogger logger1 = logger.SetNext(new ConsoleLogger(LogLevel.Debug)) as ConsoleLogger;
        }
        

        public abstract class AbstractLogger
        {
            protected LogLevel logMask;

            // The next Handler in the chain
            protected AbstractLogger next;

            public AbstractLogger(LogLevel mask)
            {
                this.logMask = mask;
            }

            /// <summary>
            /// Sets the Next logger to make a list/chain of Handlers.
            /// </summary>
            public AbstractLogger SetNext(AbstractLogger nextlogger)
            {
                next = nextlogger;
                return nextlogger;
            }

            public void Message(string msg, LogLevel severity)
            {
                if ((severity & logMask) != 0) //True only if all logMask bits are set in severity
                {
                    WriteMessage(msg);
                }
                if (next != null)
                {
                    next.Message(msg, severity);
                }
            }

            abstract protected void WriteMessage(string msg);
        }

        public class ConsoleLogger : AbstractLogger
        {
            public ConsoleLogger(LogLevel mask)
                : base(mask)
            { }

            protected override void WriteMessage(string msg)
            {
                Console.WriteLine(msg);
            }
        }

        class FileLogger : AbstractLogger
        {
            public FileLogger(LogLevel mask)
                : base(mask)
            { }

            protected override void WriteMessage(string msg)
            {
                sw.WriteLine(msg);
            }
        }

        public static void WRITE(string msg, LogLevel severity)
        {
            msg = "[" + severity.ToString().ToUpper() + "] " + msg;
            logger.Message(msg, severity);
        }

        public static void BREAK()
        {
            logger.Message("\n", LogLevel.All);
        }

        public static void Close()
        {
            sw.Dispose();
        }
    }
}
