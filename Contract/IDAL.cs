using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public  interface IDAL
    {
        string GetNotes(string path);
        void SaveNotes(string list,string path);
    }
}
