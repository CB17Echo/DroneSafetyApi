using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Services
{
    public interface IHazardsToHeatmapsResponse
    {
        HeatmapsResponse ConvertToHeatmapResponse(
            int height,
            int width,
            Polygon area,
            Dictionary<string, IEnumerable<Hazard>> hazards);
    }
}
