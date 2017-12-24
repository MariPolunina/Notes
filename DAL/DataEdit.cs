using Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DAL
{
    public class DataEdit : IDAL
    {
        public string GetNotes(string path)

        {
            //REVIEW: Могут быть исключения
            if(String.IsNullOrWhiteSpace(path))
            {
                Logger.Log.Error("Path is empty");
                throw new ArgumentNullException("Path is empty");
            }
            
            string st;

            using(StreamReader sr = new StreamReader(path))
            {
                st = sr.ReadToEnd();
            }
            return st;
        }
        public void SaveNotes(string stringlist, string path)
        {
            //REVIEW: И тут могут быть исключения
            if (String.IsNullOrWhiteSpace(path))
            {
                Logger.Log.Error("Path is empty");
                throw new ArgumentNullException("Path is empty");
            }
            if (String.IsNullOrWhiteSpace(stringlist))
            {
                Logger.Log.Error("Stringlist is empty");
                throw new ArgumentNullException("Stringlist is empty");
            }
            char[] ch = stringlist.ToCharArray();
            using(StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(ch);
            }
        }
    }
}
