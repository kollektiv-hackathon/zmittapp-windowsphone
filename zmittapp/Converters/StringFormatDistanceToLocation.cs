using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using zmittapp.DataModel;
using Windows.Devices.Geolocation;
using Microsoft.Practices.ServiceLocation;

namespace zmittapp.Converters
{
   public class StringFormatDistanceToLocation : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return value;

            var restaurant = (Restaurant)value;


            //var currentCoordinated =  ServiceLocator.Current.GetInstance<User>().CurrentCoordination;

            //return String.Format("{0} km", Math.Round(GetDistance(currentCoordinated.Longitude, restaurant.Lon, currentCoordinated.Latitude, restaurant.Lat),2));        
            return String.Format("{0} km", Math.Round(restaurant.CurrentDistanceToLocation, 2)); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

       private double GetDistance(double lon1, double lon2, double lat1, double lat2){

            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta)); dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            dist = dist * 1.609344;

            return dist; 
       }

       private double deg2rad(double deg) {
          return (deg * Math.PI / 180.0);
       }

       private double rad2deg(double rad)
       {
           return (rad / Math.PI * 180.0);
       }
    }
}

