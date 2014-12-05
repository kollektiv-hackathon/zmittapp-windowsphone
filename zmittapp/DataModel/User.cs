using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace zmittapp.DataModel
{
    public class User 
    {
        private string _uid;
        private ObservableCollection<Restaurant> _subscriptions; 

        public string Uid
        {
            get
            {
                return "lorenz.wolf";
                //return _uid; 
            }
        }

        public ObservableCollection<Restaurant> Subscriptions
        {
            get
            {
                return _subscriptions; 
            }
            set
            {
                if (_subscriptions == value) return;

                _subscriptions = value;

            }
        }
    }
}
