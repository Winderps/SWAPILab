using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SWAPILab.Models
{
    public class ListRootObject<T>
    {
        public ListRootObject()
        {
            Results = new HashSet<T>();
        }
        
        public string Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        [JsonProperty("results")]
        public IEnumerable<T> Results { get; set; }
    }
}