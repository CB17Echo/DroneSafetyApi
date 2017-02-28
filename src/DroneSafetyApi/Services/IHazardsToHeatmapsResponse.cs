using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;
using System.Collections.Generic;

namespace DroneSafetyApi.Services
{
    /// <summary>
    /// The IHazardsToHeatmapsResponse interface is capable of generating <see cref="HeatmapsResponse"/>s
    /// from a collection of <see cref="Hazard"/>s 
    /// </summary>
    public interface IHazardsToHeatmapsResponse
    {
        /// <summary>
        /// The ConvertToHeatmapResponse method creates a <see cref="HeatmapsResponse"/> for a specified area
        /// and  a collection of <see cref="Hazard"/>s 
        /// </summary>
        /// <param name="area">The bounding box that the <see cref="IHeatmap"/>s in the <see cref="HeatmapsResponse"/> covers</param>
        /// <param name="numberLonPoints">The number of points along the Longitude axis in the <see cref="IHeatmap"/>s created for
        ///  the <see cref="HeatmapsResponse"/> covers</param>
        /// <param name="hazards">A collection of <see cref="Hazard"/>s that are to be mapped onto <see cref="IHeatmap"/>s</param>
        /// <returns></returns>
        HeatmapsResponse ConvertToHeatmapResponse(
            Bounds area,
            int numberLonPoints,
            IEnumerable<Hazard> hazards);
    }
}
