using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Spatial;
using DroneSafetyApi.Models;

namespace DroneSafetyApi.Data
{
    /// <summary>
    /// The IHazardRepository interface is capable of finding <see cref="Hazard"/>s in a given circle and time
    /// </summary>
    public interface IHazardRepository
    {
        /// <summary>
        /// The GetHazardsInRadius method queries all <see cref="Hazard"/>s that lie within a circle with a given
        /// centre and radius at a specific time
        /// </summary>
        /// <param name="location">The centre of the query circle</param>
        /// <param name="radius">The radius of the query circle</param>
        /// <param name="time">The time of the query</param>
        /// <returns></returns>
        IEnumerable<Hazard> GetHazardsInRadius(Point location, int radius, DateTime time);
    }
}
