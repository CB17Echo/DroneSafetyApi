using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Spatial;
using Newtonsoft.Json;

namespace DroneSafetyApi.Models
{
    public class Hazard
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public int Severity { get; set; }
        public Polygon Area { get; set; }
    }
}
