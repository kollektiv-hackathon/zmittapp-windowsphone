using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using zmittapp.DataModel;

namespace zmittapp.ViewModel
{
    public class RestaurantByLocationViewModel : MainViewModel
    {
        private List<Restaurant> _restaurants;
        private List<Restaurant> _originalRestaurants;

        private float _currentLatitude;
        private float _currentLongitude;

        private Geocoordinate _currentCoordinates; 

        private string _keyword;


        public List<Restaurant> Restaurants
        {
            get
            {
                return _restaurants;
            }
            set
            {
                if (_restaurants == value) return;

                _restaurants = value;
                RaisePropertyChanged(() => Restaurants);
            }
        }

        public List<Restaurant> OriginalRestaurants
        {
            get
            {
                return _originalRestaurants;
            }
            set
            {
                if (_originalRestaurants == value) return;

                _originalRestaurants = value;
                RaisePropertyChanged(() => OriginalRestaurants);
            }
        }

        public string Keyword
        {
            get
            {
                return _keyword;
            }
            set
            {
                if (_keyword == value) return;

                _keyword = value;

                if (value == "" || value == null)
                {
                    Restaurants = OriginalRestaurants;
                }
                else
                {
                    Restaurants = OriginalRestaurants.Where(o => o.Name.ToUpper().Contains(value.ToUpper())).ToList(); 
                }
            }
        }
        
        public async Task GetRestaurants()
        {
            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync(new Uri("http://api.zmittapp.ch/restaurants/?_format=json"));
                result.EnsureSuccessStatusCode();

                Restaurants =  JsonConvert.DeserializeObject<IEnumerable<Restaurant>>(await result.Content.ReadAsStringAsync()).ToList();
                OriginalRestaurants = Restaurants.ToList(); 

            }
        }

        public async Task GetCurrentLocation()
        {

            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
     
            if (!((bool) settings.Values["LocationConsent"])) return;

            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(2),
                    timeout: TimeSpan.FromSeconds(10)
                    );

                User.CurrentCoordination = geoposition.Coordinate; 
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    // the application does not have the right capability or the location master switch is off
                    MessageDialog messageDialog = new MessageDialog("GPS nicht verfügbar", "Fehler");
                }
                {
                    MessageDialog messageDialog = new MessageDialog("Es ist ein Fehler augetreten. Bitte versuchen Sie später erneut", "Fehler");
                }
            }

        }
    }
}
