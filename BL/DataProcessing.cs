using Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BL
{
    public class DataProcessing : IDataProcessing
    {
        IDAL reader;

        public DataProcessing()
        {
        }

        Config config;
        public DataProcessing(Config config)
        {
            this.config=config;
            //    _logger.Info($"FileProcessing ctor with {config}");
          //  Logger.Log.Info($"FileProcessing ctor with {config}");

            if (!File.Exists(config.DataReaderAssembly))
            {
                Logger.Log.Info($"FileProcessing ctor with {config}");
                throw new ArgumentException("Can't find assembly!");
                
            }
                
            if (config == null)
            {
                Logger.Log.Error($"Config is null");
                throw new ArgumentNullException("Config is null");
             
            }
                

            Assembly assembly = null;
            Type foundClass = null;
            try
            {
                assembly = Assembly.LoadFile(config.DataReaderAssembly);
                Logger.Log.Info($"assembly {assembly.FullName} loaded");
                foundClass = assembly.GetExportedTypes().FirstOrDefault(x => x.GetInterface("IDAL") != null);
                Logger.Log.Info($"class {foundClass.FullName} loaded");
            }
            catch (Exception ex)
            {
                Logger.Log.Error($"Can't create reader {ex}");
                throw;
            }
            if (foundClass == null)
            {
                Logger.Log.Error($"Can't find class with IDAL interface");
                throw new InvalidOperationException("Can't find class with IDAL interface");
               
            }
               


             reader = Activator.CreateInstance(assembly.FullName, foundClass.FullName).Unwrap() as IDAL;
            if (reader == null)
            {
                Logger.Log.Error($"Can't create reader instance");
                throw new InvalidOperationException("Can't create reader instance");
               
            }
                

            if (String.IsNullOrWhiteSpace(config.DataPath))
            {
                Logger.Log.Error($"Config.DataPath is null!");
                throw new ArgumentNullException("Config.DataPath is null!");
                

            }
            if (!File.Exists(config.DataPath))
            {
                Logger.Log.Error($"Can't find file {config.DataPath}");
                throw new FileNotFoundException($"Can't find file {config.DataPath}");
                

            }


        }
       

       

        public void SaveNotes(string notes)
        {
            reader.SaveNotes(notes,config.DataPath);
            Logger.Log.Info("Data saved");
        }

        public void SaveNotesOnDisk(string notes)
        {
            throw new NotImplementedException();
        }

       

       

        string IDataProcessing.GetNotes()
        {
            Logger.Log.Info("Data loaded");
            return reader.GetNotes(config.DataPath);
           
        }
    }
}
