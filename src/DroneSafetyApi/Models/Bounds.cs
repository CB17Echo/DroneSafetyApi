using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Models
{
    /// <summary>
    /// The Bounds class is a model to represent a bounding box with Longitude and Latitude coordinates
    /// </summary>
    public class Bounds
    {
        /// <summary>
        /// The Min property represents the coordinates for the minimum Longitude and Latitude of the bounding box
        /// </summary>
        public Position Min { get; private set; }
        /// <summary>
        /// The Max property represents the coordinates for the maximum Longitude and Latitude of the bounding box
        /// </summary>
        public Position Max { get; private set; }
        /// <summary>
        /// Creates a new instance of the <see cref="Bounds"/> class 
        /// </summary>
        /// <param name="cornerOne">The first corner of the bounding box</param>
        /// /// <param name="cornerTwo">The second corner of the bounding box</param>
        public Bounds(Position cornerOne, Position cornerTwo)
        {
            Max = new Position(
                Math.Max(cornerOne.Longitude, cornerTwo.Longitude),
                Math.Max(cornerOne.Latitude, cornerTwo.Latitude));
            Min = new Position(
                Math.Min(cornerOne.Longitude, cornerTwo.Longitude),
                Math.Min(cornerOne.Latitude, cornerTwo.Latitude));
        }
    }
}
