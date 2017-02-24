using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Models
{
    public class CircularHazard : Hazard
    {
        public new Point Location { get; set; }
        public int Radius { get; set; } //Unit is meters
    }
}