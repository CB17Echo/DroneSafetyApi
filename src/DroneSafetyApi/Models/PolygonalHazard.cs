using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Models
{
    public class PolygonalHazard : Hazard
    {
        public new Polygon Location { get; set; }
    }
}