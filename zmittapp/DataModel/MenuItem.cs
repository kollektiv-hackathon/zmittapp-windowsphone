using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json; 

namespace zmittapp.DataModel
{
    public class MenuItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("appetizer")]
        public string Appetizer { get; set; }

        [JsonProperty("main_course")]
        public string MainCourse { get; set; }

        [JsonProperty("desert")]
        public string Desert { get; set; }

        [JsonProperty("price")]
        public float Price { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("vegetarian")]
        public bool Vegetarian { get; set; }

        [JsonProperty("vegan")]
        public bool Vegan { get; set; }

    }
}
