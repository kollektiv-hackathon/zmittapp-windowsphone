using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using zmittapp.DataModel;

namespace zmittapp.ViewModel
{
    public class RestaurantDetailViewModel : MainViewModel
    {

        private Restaurant _restaurant;
        private ICommand _getRestaurantCommand;
        private ICommand _subscribeCommand;
        private ICommand _unsubscribeCommand;
        
        public Restaurant Restaurant
        {
            get
            {
                return _restaurant;
            }
            set
            {
                if (_restaurant == value) return;

                _restaurant = value;
                RaisePropertyChanged(() => Restaurant);
            }
        }

        public List<MenuItem> MenuItems
        {
            get
            {
                if (_restaurant != null) return _restaurant.MenuItems;

                return null;                  
            }
            set
            {
                if (_restaurant.MenuItems == value) return;

                _restaurant.MenuItems = value;
                RaisePropertyChanged(() => MenuItems);
            }
        }

        //TODO: -> ServiceProxy
        private async Task GetRestaurantById(int id)
        {
         using (HttpClient client = new HttpClient())
                {

                    client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                    var result = await client.GetAsync(new Uri("http://api.zmittapp.ch/restaurants/" + id + "?_format=json"));

                    Restaurant = JsonConvert.DeserializeObject<Restaurant>(await result.Content.ReadAsStringAsync());
                    RaiseCanExecuteChanged(); 
                }
         
            await GetMenuesByRestaurantId(id);
        }

        //TODO: -> ServiceProxy
        private async Task GetMenuesByRestaurantId(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync(new Uri("http://api.zmittapp.ch/restaurants/" + id + "/menuitems?_format=json"));

                if (Restaurant != null)
                    MenuItems = new List<MenuItem>(
                        JsonConvert.DeserializeObject<IEnumerable<MenuItem>>(await result.Content.ReadAsStringAsync()));
            }
            
        }

        //TODO: -> ServiceProxy
        private async Task PutSubscription()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.PutAsync(new Uri("http://api.zmittapp.ch/restaurants/" + Restaurant.Id + "/subscribe/" + User.Uid), null);

                
               if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    await PostUser();
                    result = await client.PutAsync(new Uri("http://api.zmittapp.ch/restaurants/" + Restaurant.Id + "/subscribe/" + User.Uid), null);
                }

                result.EnsureSuccessStatusCode();

                base.User.Subscriptions.Add(Restaurant);
                RaiseCanExecuteChanged();   

            }
        }

        //TODO: -> ServiceProxy
        private async Task PutUnsubscription()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.PutAsync(new Uri("http://api.zmittapp.ch/restaurants/" + Restaurant.Id + "/unsubscribe/" + User.Uid), null);

                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    await PostUser();
                    result = await client.PutAsync(new Uri("http://api.zmittapp.ch/restaurants/" + Restaurant.Id + "/subscribe/" + User.Uid), null);
                }
               
                result.EnsureSuccessStatusCode();

                base.User.Subscriptions.Remove(Restaurant);
                RaiseCanExecuteChanged(); 
            }
        }

        //TODO: -> ServiceProxy
        private async Task PostUser()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                HttpFormUrlEncodedContent content = new HttpFormUrlEncodedContent(new[] { new KeyValuePair<string, string>("user[uid]", User.Uid.ToString()) });

                var resultUserPost = await client.PostAsync(new Uri("http://api.zmittapp.ch/user/"), content);
                resultUserPost.EnsureSuccessStatusCode();
            }
        }

        public ICommand GetRestaurantByIdCommand
        {
            get
            {
                if (_getRestaurantCommand == null)
                {
                    _getRestaurantCommand = new RelayCommand<int>(GetRestaurantByIdExecute);
                }
                return _getRestaurantCommand;
            }
        }

        private async void GetRestaurantByIdExecute(int id)
        {
            await GetRestaurantById(id); 
        }

        public ICommand SubscribeCommand
        {
            get
            {
                if (_subscribeCommand == null)
                {
                    _subscribeCommand = new RelayCommand(SubscribeExecute, CanSubscribe);
                }
                return _subscribeCommand;
            }
        }

        public ICommand UnsubscribeCommand
        {
            get
            {
                if (_unsubscribeCommand == null)
                {
                    _unsubscribeCommand = new RelayCommand(UnsubscribeExecute, CanUnsubscribe);
                }
                return _unsubscribeCommand;
            }
        }

        private async void SubscribeExecute()
        {
            await PutSubscription();
        }

        private bool CanSubscribe()
        {
            if (Restaurant == null) return false; 
            return base.User.Subscriptions.Where(o => o.Id == Restaurant.Id).SingleOrDefault() == null;
        }

        private async void UnsubscribeExecute()
        {
            await PutUnsubscription();
        }

        private bool CanUnsubscribe()
        {
            return !CanSubscribe();
        }

        private void RaiseCanExecuteChanged()
        {
            ((RelayCommand)UnsubscribeCommand).RaiseCanExecuteChanged();
            ((RelayCommand)SubscribeCommand).RaiseCanExecuteChanged();
        }
    }

}
