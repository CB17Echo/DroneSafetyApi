using System;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Models
{
    public class Hazard
    {
        public int Severity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Shape { get; set; }
        public Geometry Location { get; set; }
        public string DataType { get; set; }
    }
}
