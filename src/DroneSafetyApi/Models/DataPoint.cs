using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Models
{
    public class DataPoint
    {
        public int Data_ID { get; set; }
        public string DataType { get; set; }
        public string Shape { get;  set; }
        public int Time { get; set; }
        public int Severity { get; set; }
        public Geometry Location { get; set; }
    }
}
