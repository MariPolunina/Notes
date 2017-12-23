using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
   public interface IDataProcessing
    {
        string GetNotes();
        void SaveNotesOnDisk(string notes);
        void SaveNotes(string notes);


    }
}
