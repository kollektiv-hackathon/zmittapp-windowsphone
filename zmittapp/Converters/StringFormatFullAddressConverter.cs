using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using zmittapp.DataModel;

namespace zmittapp.Converters
{
   public class StringFormatFullAddressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return value;

            var restaurant = (Restaurant)value; 

             return String.Format("{0}, {1}-{2} {3}", restaurant.Address, restaurant.Country, restaurant.Zip,restaurant.City); 
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

