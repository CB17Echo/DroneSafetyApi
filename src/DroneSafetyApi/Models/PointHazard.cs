using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Models
{
    /// <summary>
    /// The PointHazard class is a model to hold additional state for point-based hazards
    /// </summary>
    public class PointHazard : Hazard
    {
        /// <summary>
        /// The Location property holds the coordinate point for the given hazard
        /// </summary>
        public new Point Location { get; set; }
    }
}