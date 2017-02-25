using System.Collections.Generic;
using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Services
{
    public abstract class HazardsToHeatmapsResponseNoCompositionSendAllSources : IHazardsToHeatmapsResponse
    {
        public HeatmapsResponse ConvertToHeatmapResponse(Bounds area, int width, int height, IEnumerable<Hazard> hazards)
        {
            Dictionary<string, List<Hazard>> hazardsBySource = new Dictionary<string, List<Hazard>>();
            foreach (Hazard hazard in hazards)
            {
                List<Hazard> list;
                hazardsBySource.TryGetValue(hazard.DataType, out list);
                if (list == null)
                {
                    list = new List<Hazard>();
                    hazardsBySource.Add(hazard.DataType, list);
                }
                list.Add(hazard);
            }

            Dictionary<string, IEnumerable<HeatmapPoint>> heatmaps = new Dictionary<string, IEnumerable<HeatmapPoint>>();
            foreach (KeyValuePair<string, List<Hazard>> pair in hazardsBySource)
            {
                heatmaps.Add(pair.Key, ConvertToHeatmap(area, width, height, pair.Value).GetHeatMapPoints());
            }

            return new HeatmapsResponse
            {
                NumSources = heatmaps.Count,
                Sources = heatmaps.Keys,
                Heatmaps = heatmaps
            };
        }

        public abstract HeatMap ConvertToHeatmap(Bounds area, int width, int height, IEnumerable<Hazard> hazards);
    }
}
