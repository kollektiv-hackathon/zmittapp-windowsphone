﻿using System;
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
   public class NoMenuItemsVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return value;

            List<MenuItem> menus = value as List<MenuItem>;

            return (menus.Count == 0) ? Visibility.Visible : Visibility.Collapsed; 
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

