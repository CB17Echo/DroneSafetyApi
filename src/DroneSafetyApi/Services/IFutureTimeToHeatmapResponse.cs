using DroneSafetyApi.Data;
using DroneSafetyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Services
{
    public interface IFutureTimeToHeatmapResponse
    {
        HeatmapsResponse ConvertToHeatmapResponse(
            HeatmapsQuery query, IHazardsToHeatmapsResponse hazardsToHeatmaps, IHazardRepository hazards);
    }
}
