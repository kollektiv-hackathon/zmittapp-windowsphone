using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Linq; 
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using zmittapp.DataModel;

namespace zmittapp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private ObservableCollection<Restaurant> _restaurants;
        private Restaurant _restaurant;
        
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

        public Restaurant Restaurant
        {
            get
            {
                return _restaurant;
            }
            set{
                if (_restaurant == value) return;

                _restaurant = value;
                RaisePropertyChanged(() => Restaurant);
            }
        }

        public ObservableCollection<MenuItem> MenuItems
        {
            get
            {
                return _restaurant.MenuItems;
            }
            set
            {
                if (_restaurant.MenuItems == value) return;

                _restaurant.MenuItems = value;
                RaisePropertyChanged(() => MenuItems);
            }
        }

        public MainViewModel()
        {
            GetRestaurant(); 
        }

        private async Task GetRestaurant()
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

        public async Task GetRestaurantById(int id)
        {
            if (Restaurants.Any(o => o.Id == id))
            {
                Restaurant = Restaurants.Where(o => o.Id == id).SingleOrDefault();
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {

                    client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                    var result = await client.GetAsync(new Uri("http://api.zmittapp.ch/restaurants/" + id + "?_format=json"));

                    Restaurant = JsonConvert.DeserializeObject<Restaurant>(await result.Content.ReadAsStringAsync());
                }
            }
            
            GetMenuesByRestaurantId(id);
            
        }

        public async Task GetMenuesByRestaurantId(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync(new Uri("http://api.zmittapp.ch/restaurants/" + id + "/menuitems?_format=json"));

                if(Restaurant != null)
                    MenuItems = new ObservableCollection<MenuItem>(
                        JsonConvert.DeserializeObject<IEnumerable<MenuItem>>(await result.Content.ReadAsStringAsync()));
                   
            }
        }
    }
}