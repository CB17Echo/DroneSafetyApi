using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Models
{
    /// <summary>
    /// The PolygonalHazard class is a model to hold additional state for polygon-based hazards
    /// </summary>
    public class PolygonalHazard : Hazard
    {
        /// <summary>
        /// The Location property holds the coordinates of polygon for the given hazard
        /// </summary>
        public new Polygon Location { get; set; }
    }
}