using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace ArbitrageBot.Util
{
    public static class Config
    {
        static Dictionary<string, string> properties;

        static string[] Args { get; set; }

        static string ConfigFilePath { get; set; }

        public static void ImportProperties(string file = "config.txt")
        {
            Regex pattern = new Regex("([A-Za-z0-9])+=([A-Za-z0-9])+");
            properties = File.ReadAllLines(file)
                        .Select(ln => ln.Replace(" ", ""))
                        .Where(ln => pattern.IsMatch(ln))
                        .Select(ln => new KeyValuePair<string, string>(
                                                ln.Split('=')[0],
                                                ln.Split('=')[1]))
                        .ToDictionary(x => x.Key, x => x.Value);
        }

        public static string Get(string key)
        {
            return properties[key];
        }

        public static bool ContainsKey(string key)
        {
            return properties.ContainsKey(key);
        }

        public static void Add(string key, string value)
        {
            properties.Add(key, value);
        }

        public static string GetLogFilePath()
        {
            return properties["logFile"];
        }
    }
}
