using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Notes.Converters
{
  public  class ConvertDate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value!=null)
            {
                DateTime dateTime = (DateTime) value;
                if(dateTime.Year==DateTime.Now.Year&& dateTime.Month == DateTime.Now.Month)
                {
                    if(dateTime.Day==DateTime.Now.Day)
                    {
                        return "Сегодня";
                    }
                    if(dateTime.Day==DateTime.Now.AddDays(-1).Day)
                    {
                        return "Вчера";
                    }
                    
                }
                return dateTime.ToLongDateString();
            }
            return  "Сегодня"; ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
