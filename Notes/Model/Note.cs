using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Notes.Model
{
    public class Note : INotifyPropertyChanged
    {

        public DateTime? date;
        public String description;

        public Note()
        {
           
        }
        [XmlIgnore]
        public DateTime? Date
        {
            get => date; set
            {
                date = value;
                DoPropertyChanged("Date");
            }
        }
        [XmlIgnore]
        public string Description
        {
            get => description; set
            {
                description = value;
                Date = DateTime.Now;
                DoPropertyChanged("Description");

            }
        }

        #region
        public event PropertyChangedEventHandler PropertyChanged;
        public void DoPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion    
    }
}
