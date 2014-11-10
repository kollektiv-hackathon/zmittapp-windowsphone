using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel; 

namespace zmittapp.DataModel
{
    public class Restaurant
    {
        private ObservableCollection<MenuItem> _menues;
        
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("lat")]
        public float Lat { get; set; }

        [JsonProperty("lon")]
        public float Lon { get; set; }

        public String Position
        {
            get
            {
                return String.Format("{0}, {1}", Lat, Lon);
            }
        }

        public ObservableCollection<MenuItem> MenuItems
        {
            get
            {
                return _menues;
            }
            set
            {
                if (_menues == value) return;

                _menues = value;
            }
        }


    }
}
