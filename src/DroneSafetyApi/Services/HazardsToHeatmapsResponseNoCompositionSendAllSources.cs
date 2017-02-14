using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Services
{
    public abstract class HazardsToHeatmapsResponseNoCompositionSendAllSources : IHazardsToHeatmapsResponse
    {
        public HeatmapsResponse ConvertToHeatmapResponse(
            int height,
            int width,
            BoundingBox area,
            Dictionary<string, IEnumerable<Hazard>> hazards
            )
        {
            Dictionary<string, Heatmap> heatmaps = new Dictionary<string, Heatmap>();
            foreach (var item in hazards)
            {
                heatmaps.Add(item.Key, ConvertToHeatmap(height, width, area, item.Value));
            }
            return new HeatmapsResponse
            {
                NumSources = hazards.Count,
                Sources = hazards.Keys,
                Height = height,
                Width = width,
                Heatmaps = heatmaps
            };
        }

        public abstract Heatmap ConvertToHeatmap(int height, int width, BoundingBox area, IEnumerable<Hazard> hazards);
        public abstract HeatmapsResponse ConvertToHeatmapResponse(int height, int width, Polygon area, Dictionary<string, IEnumerable<Hazard>> hazards);
    }
}
