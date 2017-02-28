using DroneSafetyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Services
{
    /// <summary>
    /// The IHeatmapFactory interface is capable of generating new <see cref="IHeatmap"/>s
    /// </summary>
    public interface IHeatmapFactory
    {
        /// <summary>
        /// The CreateHeatmap method generates a new <see cref="IHeatmap"/> that represents a bounding box area
        /// </summary>
        /// <param name="area">The bounding box that the new <see cref="IHeatmap"/> instance covers</param>
        /// <param name="numberLonPoints">The number of points along the Longitude axis in the new <see cref="IHeatmap"/> instance</param>
        /// <returns>A new <see cref="IHeatmap"/> </returns>
        IHeatmap CreateHeatmap(Bounds area, int numberLonPoints);
    }
}
