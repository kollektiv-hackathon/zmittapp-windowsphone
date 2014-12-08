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
using System.Windows.Input;
using zmittapp.Common;

namespace zmittapp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        //private ObservableCollection<Restaurant> _restaurants;
        //private Restaurant _restaurant;
        //private ICommand _subscribeCommand;
        //private ICommand _unsubscribeCommand;
        private User _user; 

        //public ObservableCollection<Restaurant> Restaurants
        //{
        //    get
        //    {
        //        return _restaurants;
        //    }
        //    set
        //    {
        //        if (_restaurants == value) return;

        //        _restaurants = value;
        //        RaisePropertyChanged(() => Restaurants);
        //    }
        //}

        public ObservableCollection<Restaurant> Subscriptions
        {
            get
            {
                if (User.Subscriptions == null)
                {
                    User.Subscriptions = new ObservableCollection<Restaurant>();
                    User.Subscriptions.Add(new Restaurant { Name = "Keine Subscriptions",  });
                }
                return User.Subscriptions;
            }
            set
            {
                if (User.Subscriptions == value) return;

                User.Subscriptions = value;
                RaisePropertyChanged(() => Subscriptions);
            }
        }

        //public Restaurant Restaurant
        //{
        //    get
        //    {
        //        return _restaurant;
        //    }
        //    set{
        //        if (_restaurant == value) return;

        //        _restaurant = value;
        //        RaisePropertyChanged(() => Restaurant);
        //    }
        //}

        //public ObservableCollection<MenuItem> MenuItems
        //{
        //    get
        //    {
        //        return _restaurant.MenuItems;
        //    }
        //    set
        //    {
        //        if (_restaurant.MenuItems == value) return;

        //        _restaurant.MenuItems = value;
        //        RaisePropertyChanged(() => MenuItems);
        //    }
        //}

        public User User
        {
            get
            {
                return _user;
            }
            set
            {
                if (_user == value) return;

                _user = value;
               RaisePropertyChanged(() => User);
            } 
        }

        public MainViewModel()
        {
            User = new User();
            GetSubsciptionsByUser();
        }

        private async Task GetSubsciptionsByUser()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync(new Uri(String.Format("http://api.zmittapp.ch/user/{0}/subscriptions?_format=json", User.Uid)));
                result.EnsureSuccessStatusCode();

                Subscriptions = new ObservableCollection<Restaurant>(
                   JsonConvert.DeserializeObject<IEnumerable<Restaurant>>(await result.Content.ReadAsStringAsync())); 

            }
        }

        //public async Task GetRestaurants()
        //{
        //    using (HttpClient client = new HttpClient())
        //    {

        //        client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

        //        var result = await client.GetAsync(new Uri("http://api.zmittapp.ch/restaurants/?_format=json"));
        //        result.EnsureSuccessStatusCode();

        //        Restaurants = new ObservableCollection<Restaurant>(
        //           JsonConvert.DeserializeObject<IEnumerable<Restaurant>>(await result.Content.ReadAsStringAsync()));
        //    }
        //}

        

        //public async Task GetMenuesByRestaurantId(int id)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

        //        var result = await client.GetAsync(new Uri("http://api.zmittapp.ch/restaurants/" + id + "/menuitems?_format=json"));

        //        if(Restaurant != null)
        //            MenuItems = new ObservableCollection<MenuItem>(
        //                JsonConvert.DeserializeObject<IEnumerable<MenuItem>>(await result.Content.ReadAsStringAsync()));
        //    }
        //}

        //public async Task PutSubscription()
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

        //        var result = await client.PutAsync(new Uri("http://api.zmittapp.ch/restaurants/" + Restaurant.Id + "/subscribe/" + User.Uid),null);

        //        if (result.StatusCode == HttpStatusCode.NotFound)
        //        {
        //            await PostUser(); 
        //            await PutSubscription(); //TODO: refactor
        //        }

        //        User.Subscriptions.Add(Restaurant); 

        //     }
        //}

        //public async Task PutUnsubscription()
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

        //        var result = await client.PutAsync(new Uri("http://api.zmittapp.ch/restaurants/" + Restaurant.Id + "/unsubscribe/" + User.Uid), null);

        //        if (result.StatusCode == HttpStatusCode.NotFound)
        //        {
        //            await PostUser();
        //            await PutUnsubscription(); //TODO: refactor
        //        }

        //        User.Subscriptions.Remove(Restaurant); 

        //    }
        //}

        //public async Task PostUser(){
        //    using (HttpClient client = new HttpClient())
        //    {
        //        HttpFormUrlEncodedContent content = new HttpFormUrlEncodedContent(new []{new KeyValuePair<string, string>("uid", User.Uid.ToString())});

        //        var resultUserPost = await client.PostAsync(new Uri("http://api.zmittapp.ch/user/"), content);
        //        resultUserPost.EnsureSuccessStatusCode();
        //    }
        //}

        //public ICommand SubscribeCommand
        //{
        //    get
        //    {
        //        if (_subscribeCommand == null)
        //        {
        //            _subscribeCommand = new RelayCommand(SubscribeExecute, CanSubscribe);
        //        }
        //        return _subscribeCommand;
        //    }
        //}

        //public ICommand UnsubscribeCommand
        //{
        //    get
        //    {
        //        if (_subscribeCommand == null)
        //        {
        //            _subscribeCommand = new RelayCommand(UnsubscribeExecute, CanSubscribe);
        //        }
        //        return _subscribeCommand;
        //    }
        //}

        //private async void SubscribeExecute()
        //{
        //    await PutSubscription(); 
        //}

        // private bool CanSubscribe()
        //{
        //    //TODO: only when not subscribed yet 
        //     return true; 
        //}

        // private async void UnsubscribeExecute()
        // {
        //     await PutUnsubscription();
        // }
    }
}