using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ClassLibrary1
{
    [Serializable]
    public class TestTime
    {
        List<string> info;

        public TestTime()
        {
            info = new List<String>();
        }

        public void Add(string record)
        {
            info.Add(record);
        }

        public static bool Save(string filename, TestTime obj)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(filename, FileMode.Create);
                bf.Serialize(fs, obj);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool Load(string filename, ref TestTime obj)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(filename, FileMode.Create);
                obj = bf.Deserialize(fs) as TestTime;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            string res = "";
            foreach (string elem in info)
            {
                res += elem + "\n";
            }
            return res;
        }
    };
}