using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using zmittapp.DataModel;

namespace zmittapp.ViewModel
{
    public class RestaurantIndexViewModel : MainViewModel
    {
        private ObservableCollection<Restaurant> _restaurants;

        public ObservableCollection<Restaurant> Restaurants
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

        public async Task GetRestaurants()
        {
            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync(new Uri("http://api.zmittapp.ch/restaurants/?_format=json"));
                result.EnsureSuccessStatusCode();

                Restaurants = new ObservableCollection<Restaurant>(
                   JsonConvert.DeserializeObject<IEnumerable<Restaurant>>(await result.Content.ReadAsStringAsync()));
            }
        }
    }
}
