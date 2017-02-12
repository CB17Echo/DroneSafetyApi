using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Services
{
    public abstract class HazardsToHeatmapResponseStrategy
    {
        public HeatmapResponse ConvertToHeatmapResponse(
            int height,
            int width,
            Polygon area,
            Dictionary<string, IEnumerable<Hazard>> hazards
            )
        {
            Dictionary<string, Heatmap> heatmaps = new Dictionary<string, Heatmap>();
            foreach (var item in hazards)
            {
                heatmaps.Add(item.Key, ConvertToHeatmap(height, width, item.Value));
            }
            return new HeatmapResponse
            {
                NumSources = hazards.Count,
                Sources = hazards.Keys,
                Height = height,
                Width = width,
                Heatmaps = heatmaps
            };
        }

        public abstract Heatmap ConvertToHeatmap(int height, int width, IEnumerable<Hazard> hazards);
         
    }
}
