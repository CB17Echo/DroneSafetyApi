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
        [JsonProperty(PropertyName = "id")]
        public string DataType { get; set; }
        public string Shape { get;  set; }
        public int Time { get; set; }
        public int Severity { get; set; }
        public Geometry Location { get; set; }
        public int Data_ID { get; set; }

    }
}
