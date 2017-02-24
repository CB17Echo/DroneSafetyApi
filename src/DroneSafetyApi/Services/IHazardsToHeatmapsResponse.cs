using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;
using System.Collections.Generic;

namespace DroneSafetyApi.Services
{
    public interface IHazardsToHeatmapsResponse
    {
        HeatmapsResponse ConvertToHeatmapResponse(
            double resolution,
            BoundingBox area,
            IEnumerable<Hazard> hazards);
    }
}
