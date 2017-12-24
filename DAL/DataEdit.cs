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
            char[] ch = stringlist.ToCharArray();
            using(StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(ch);
            }
        }
    }
}
