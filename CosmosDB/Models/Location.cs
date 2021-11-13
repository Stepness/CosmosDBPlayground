using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDB.Models
{
    public class Location
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        public string Name { get; set; }
    }
}
