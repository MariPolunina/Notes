using BL;
using Contract;
using Microsoft.Office.Interop.Word;
using Notes.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace Notes
{
   public class MainViewModel:INotifyPropertyChanged
    {
        

        public MainViewModel()
        {

            Logger.InitLogger();
            if (string.IsNullOrWhiteSpace(Properties.Resources.DataConnection))
            {
                Logger.Log.Error("Data connection not found!");
                throw new ArgumentNullException("Data connection not found!");
              
            }

            if (string.IsNullOrWhiteSpace(Properties.Resources.DALAssembly))
            {
                Logger.Log.Error("DALAssembly setting not found!");
                throw new ArgumentNullException("DALAssembly setting not found!");
                
            }
             

            if (string.IsNullOrWhiteSpace(Properties.Resources.ReaderType))
            {
                Logger.Log.Error("ReaderType setting not found!");
                throw new ArgumentNullException("ReaderType setting not found!");
             
            }
               


            Environment.CurrentDirectory = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location);

            Config cfg = new Config();
            cfg.DataPath = Path.GetFullPath(Properties.Settings.Default.DataConnection);
            cfg.DataReaderAssembly = Path.GetFullPath(Properties.Settings.Default.DALAssembly);
            cfg.DataReader = Path.GetFullPath(Properties.Settings.Default.ReaderType);

            dataProcessing = new DataProcessing(cfg);
            InitCollection(dataProcessing.GetNotes());
            
        }
        private IDataProcessing dataProcessing;


        private ObservableCollection<Note> notes;
        public ObservableCollection<Note> Notes
        {
            get => notes;
            set
            {
                notes = value;
                NotesForList = notes;
                DoPropertyChanged("Notes");
            }
        }


        private ObservableCollection<Note> notesForList;
        public ObservableCollection<Note> NotesForList
        {
            get => notesForList;
            set
            {
                notesForList = value;
                DoPropertyChanged("NotesForList");
            }
        }


        private string textSearch;
        public string TextSearch {
            get => textSearch;
            set
            {
                textSearch = value;
                if (string.IsNullOrWhiteSpace(textSearch))
                {
                    NotesForList = notes;
                }
                else
                {
                    NotesForList = new ObservableCollection<Note>( notes.Where(p => p.Date!=null&&p.Description!=null &&p.Description.ToLower().Contains(textSearch)));
                }
                DoPropertyChanged(textSearch);
            }
        }


        private Note selectedNote;
        public Note SelectedNote { get => selectedNote; set
            {
                selectedNote = value;               
                DoPropertyChanged("SelectedNote");
            }
        }


       
        void InitCollection(string st)
        {
            if(string.IsNullOrWhiteSpace(st))
            {
                Notes = new ObservableCollection<Note>();
            }
            else
            {
                XmlSerializer _serializer = new XmlSerializer(typeof(List<Note>));
                using (var stream = new MemoryStream())
                {
                    var bytes = Encoding.UTF8.GetBytes(st);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Position = 0;
                    try
                    {
                      Notes =new ObservableCollection<Note>( (List<Note>)_serializer.Deserialize(stream));
                    }
                    catch(Exception)
                    {
                        Notes = new ObservableCollection<Note>();
                    }

                }
            }
        }


        private RelayCommand removeNote;
        public RelayCommand RemoveNote
        {
            get
            {
                return removeNote ??
                    (removeNote = new RelayCommand(obj =>
                    {
                        if(selectedNote!=null)
                        {
                            Notes.Remove(SelectedNote);
                            SelectedNote = null;
                            
                        }
                    }
                    ));
            }
        }

        private RelayCommand addNote;
        public RelayCommand AddNote
        {
            get
            {
                return addNote ??
                    (addNote = new RelayCommand(obj =>
                    {
                        Note item = new Note();
                        Notes.Insert(0,item);
                        SelectedNote = item;
                    }
                    ));
            }
        }
        private RelayCommand saveNoteDoc;
        public RelayCommand SaveNoteDoc
        {
            get
            {
                return saveNoteDoc ??
                    (saveNoteDoc = new RelayCommand(obj =>
                    {
                        string textDoc="";
                        foreach(var item in NotesForList)
                        {
                            textDoc = textDoc + item.Date.ToString() + "\n" +
                            item.Description+"\n-------------------------------------------\n";
                        }

                        try
                        {
                            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
                            Document doc = app.Documents.Add(Visible: true);
                            Range text = doc.Range();
                            text.Text = textDoc;
                            doc.Save();
                            doc.Close();
                            app.Quit();
                            Logger.Log.Info("Date saved in Doc");
                        }
                        catch(InvalidCastException)
                        {
                            MessageBox.Show("Активируйте Word");
                            Logger.Log.Error("Word don't activate");
                        }
                        catch(COMException)
                        {
                            MessageBox.Show("Не удалось сохранить");
                            Logger.Log.Error("failed to save");
                        }
                       
                        
                    }
                    ));
            }
        }

        

        private RelayCommand saveNotes;
        public RelayCommand SaveNotes
        {
            get
            {
                return saveNotes ??
                    (saveNotes = new RelayCommand(obj =>
                    {
                        XmlSerializer _serializer = new XmlSerializer(typeof(List<Note>));
                        string listNotes;
                       
                        using (var stream = new MemoryStream())
                        {
                            _serializer.Serialize(stream, notes.ToList());
                            stream.Position = 0;
                            listNotes = Encoding.UTF8.GetString(stream.GetBuffer());
                            dataProcessing.SaveNotes(listNotes);
                        }
                    }
                    ));
            }
        }

        #region
        public event PropertyChangedEventHandler PropertyChanged;
        public void DoPropertyChanged(string name)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}
