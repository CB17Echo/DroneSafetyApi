using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Spatial;
using Newtonsoft.Json;

namespace DroneSafetyApi.Models
{
    public class DataPoint
    {
        // TODO: Consider whether or not we need this
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string DataType { get; set; }
        public string Shape { get;  set; }
        public int Time { get; set; }
        public int Severity { get; set; }
        public Geometry Location { get; set; }
        // TODO: JsonProperty id? What is this thing doing?
        public int Data_ID { get; set; }
    }
}
