using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Models
{
    public class PointHazard : Hazard
    {
        public new Point Location { get; set; }
    }
}