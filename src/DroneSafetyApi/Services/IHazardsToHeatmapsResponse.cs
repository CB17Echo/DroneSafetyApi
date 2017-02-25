using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;
using System.Collections.Generic;

namespace DroneSafetyApi.Services
{
    public interface IHazardsToHeatmapsResponse
    {
        HeatmapsResponse ConvertToHeatmapResponse(
            Bounds area,
            int width,
            int height,
            IEnumerable<Hazard> hazards);
    }
}
