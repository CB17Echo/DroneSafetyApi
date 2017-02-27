using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Models
{
    /// <summary>
    /// The CircularlHazard class is a model to hold additional state for circular-based hazards
    /// </summary>
    public class CircularHazard : Hazard
    {
        /// <summary>
        /// The Location property holds the coordinate of the centre of the circle for the given hazard
        /// </summary>
        public new Point Location { get; set; }
        /// <summary>
        /// The Radius property represents the radius in meters of the given hazard
        /// </summary>
        public int Radius { get; set; } 
    }
}