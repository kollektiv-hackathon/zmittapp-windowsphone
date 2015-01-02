using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;


namespace zmittapp.DataModel
{
    public class User 
    {
        private string _uid;
        private List<Restaurant> _subscriptions;
        private Geocoordinate _currentLocation; 

        public string Uid
        {
            get
            {
                return "lorenz.wolf";
                //return _uid; 
            }
        }

        public List<Restaurant> Subscriptions
        {
            get
            {
                if (_subscriptions == null) _subscriptions = new List<Restaurant>(); 

                return _subscriptions; 
            }
            set
            {
                if (_subscriptions == value) return;
              
                _subscriptions = value;

            }
        }

        public Geocoordinate CurrentCoordination
        {
            get
            {
                return _currentLocation;
            }
            set
            {
                if (_currentLocation == value) return;

                _currentLocation = value; 
            }
        }
    }
}
