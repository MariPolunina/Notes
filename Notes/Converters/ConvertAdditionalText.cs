using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Notes.Converters
{
  public  class ConvertAdditionalText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace((string)value))
            {
                return "Нет доп. текста";
            }
            else
            {
                string t = (string)value;
                if (t.IndexOf("\n") == -1)
                {
                    if (t.Length > 10)
                    {
                        return t.Substring(1, 10) + "...";
                    }
                    else
                    {
                        return t;
                    }

                }
                else
                {
                    t = t.Substring(0, t.IndexOf("\n") - 1);
                    if (t.Length > 10)
                    {
                        return t.Substring(1, 10) + "...";
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
