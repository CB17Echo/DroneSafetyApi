using System;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Models
{
    /// <summary>
    /// The Hazard class is a model to hold the state of queried hazards
    /// </summary>
    public class Hazard
    {
        /// <summary>
        /// The Severity property represents the hazard level of the given hazard
        /// </summary>
        public int Severity { get; set; }
        /// <summary>
        /// The StartTime property represents the time that the given hazard starts
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// The EndTime property represents the time that the given hazard ends
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// The Shape property represents the shape illustration of the given hazard.
        /// The possible shapes are points, circles, and polygons
        /// </summary>
        public string Shape { get; set; }
        /// <summary>
        /// The Location property holds the actual geometric coordinates of the given hazard
        /// </summary>
        public Geometry Location { get; set; }
        /// <summary>
        /// The DataType property represents the data source of the given hazard such as Wifi or Bus
        /// </summary>
        public string DataType { get; set; }
    }
}
