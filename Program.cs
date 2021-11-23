using System;
using System.IO;
using System.Collections.Generic;

namespace ConfigWriter
{
    class Program
    {
        static void Main(string[] args)
        {
            var cfg = new ConfigWriter("config.cfg");
            if (!cfg.ContainsKey("volume"))
            {   
                cfg.Add("volume", "42");
            }
            else 
            {
                cfg.ChangeValue("volume", new Random().Next(0,101).ToString());
            }
            cfg.Save();
            // cfg.Reset();
        }
    }


    class ConfigWriter
    {
        private Dictionary<string, string> mData;
        private string filePath;

        public ConfigWriter(string pathToFile)
        {
            mData = new Dictionary<string, string>();
            filePath = pathToFile;
            if (File.Exists(pathToFile))
            {
                LoadFile(pathToFile);
            }
            else
            {
                File.Create(pathToFile).Close();
            }
        }

        public void Save()
        {
            CreateFileIfNotExists();
            try
            {
                using(StreamWriter sw = new StreamWriter(filePath))
                {
                    foreach (var item in mData)
                    {
                        sw.WriteLine($"{item.Key}:{item.Value}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        public void Reset()
        {
            mData.Clear();
            Save();
        }

        public string GetValue(string key)
        {
            return mData[key];
        }

        public bool ContainsKey(string key)
        {
            return mData.ContainsKey(key);
        }

        public void ChangeValue(string key, string value)
        {
            mData[key] = value;
        }

        public void Add(string key, string val)
        {
            mData.Add(key, val);
        }

        public void Remove(string key)
        {
            mData.Remove(key);
        }

        private void LoadFile(string pathToFile)
        {
            string line;
            try
            {
                using (StreamReader sr = new StreamReader(pathToFile))
                {
                    line = sr.ReadLine();
                    while (line != null)
                    {
                        var lineData = line.Split(':');
                        var key = lineData[0];
                        var val = lineData[1];
                        mData.Add(key, val);

                        line = sr.ReadLine();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        private void CreateFileIfNotExists()
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
        }
    }
}
