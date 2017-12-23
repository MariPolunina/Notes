using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Notes.Converters
{
    public class ConverterText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace((string)value))
            {
                return "Новая Заметка";
            }
            else
            {
                string t = (string)value;
                if (t.IndexOf("\n")== -1)
                {
                   if(t.Length>19)
                    {
                        return t.Substring(1, 19) + "...";
                    }else
                    {
                        return t;
                    }
                   
                }else
                {
                    t = t.Substring(0, t.IndexOf("\n")-1);
                    if (t.Length > 19)
                    {
                        return t.Substring(1, 19) + "...";
                    }
                    else
                    {
                        return t;
                    }
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
