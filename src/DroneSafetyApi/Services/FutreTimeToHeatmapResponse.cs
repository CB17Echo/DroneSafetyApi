using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroneSafetyApi.Models;
using DroneSafetyApi.Data;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Services
{
    public class FutureTimeToHeatmapResponse : IFutureTimeToHeatmapResponse
    {
        const int ComparisonNumber = 8;
        public HeatmapsResponse ConvertToHeatmapResponse(HeatmapsQuery query, IHazardsToHeatmapsResponse hazardsToHeatmaps, IHazardRepository hazards)
        {

            IEnumerable<Hazard> hazard = hazards.GetHazardsInRadius(query.Centre, query.Radius, query.Time);
            HeatmapsResponse response = hazardsToHeatmaps.ConvertToHeatmapResponse(query.Area, query.NumberLonPoints, hazard);

            DateTime current = DateTime.Now;
            TimeSpan difference = query.Time - current;

            HeatmapsResponse[] responses = new HeatmapsResponse[ComparisonNumber];

            for (int i = 0; i < ComparisonNumber; i++)
            {
                var intersectionHazards = hazards.GetHazardsInRadius(query.Centre, query.Radius, current.AddDays(-7 * i));
                responses[i] = hazardsToHeatmaps.ConvertToHeatmapResponse(
                query.Area,
                query.NumberLonPoints,
                intersectionHazards
                );
            }

            IEnumerable<string> DataTypes = responses[0].Sources;

            foreach (string datatype in DataTypes)
            {
                if (!response.Sources.Contains(datatype))
                {

                    int minimumScore = int.MaxValue;
                    int minId = 0;
                    for (int i = 1; i < ComparisonNumber; i++)
                    {
                        if (responses[i].Heatmaps.ContainsKey(datatype))
                        {
                            int score = CompareHeatmaps(responses[0].Heatmaps[datatype], responses[i].Heatmaps[datatype]);
                            if (score < minimumScore)
                            {
                                minimumScore = score;
                                minId = i;
                            }
                        }
                    }

                    var intersectionHazards = hazards.GetTypeHazardsInRadius(query.Centre, query.Radius, current.AddDays(-7 * minId) + difference, datatype);
                    var bestResponse = hazardsToHeatmaps.ConvertToHeatmapResponse(query.Area, query.NumberLonPoints, intersectionHazards);

                    response.Heatmaps.Concat(bestResponse.Heatmaps);
                    response.Sources.Concat(bestResponse.Sources);
                    response.NumSources++;
                }
            }


            return response;
        }

        public int CompareHeatmaps(IEnumerable<HeatmapPoint> heatmap1, IEnumerable<HeatmapPoint> heatmap2)
        {
            int score = 0;

            Dictionary<Position, int> difference = new Dictionary<Position, int>();

            foreach (HeatmapPoint point in heatmap1)
            {
                difference.Add(new Position(point.X, point.Y), point.Value);
            }

            foreach (HeatmapPoint point in heatmap2)
            {
                if (difference.ContainsKey(new Position(point.X, point.Y)))
                {
                    difference[new Position(point.X, point.Y)] -= point.Value;
                }
                else
                {
                    difference.Add(new Position(point.X, point.Y), point.Value);
                }
            }

            foreach (int value in difference.Values)
            {
                score += (value * value);
            }

            return score;
        }

    }
}
