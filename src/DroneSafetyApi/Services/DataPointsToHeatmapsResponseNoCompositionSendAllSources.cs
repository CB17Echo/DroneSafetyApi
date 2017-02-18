using System.Collections.Generic;
using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Services
{
    public abstract class DataPointsToHeatmapsResponseNoCompositionSendAllSources : IDataPointsToHeatmapsResponse
    {
        public HeatmapsResponse ConvertToHeatmapResponse(int width, int height, BoundingBox area, IEnumerable<DataPoint> dataPoints)
        {
            Dictionary<string, List<DataPoint>> dataPointsBySource = new Dictionary<string, List<DataPoint>>();
            foreach (DataPoint datapoint in dataPoints)
            {
                List<DataPoint> list;
                dataPointsBySource.TryGetValue(datapoint.DataType, out list);
                if (list == null)
                {
                    list = new List<DataPoint>();
                    dataPointsBySource.Add(datapoint.DataType, list);
                }
                list.Add(datapoint);
            }

            Dictionary<string, IEnumerable<HeatmapPoint>> heatmaps = new Dictionary<string, IEnumerable<HeatmapPoint>>();
            foreach (KeyValuePair<string, List<DataPoint>> pair in dataPointsBySource)
            {
                heatmaps.Add(pair.Key, ConvertToHeatmap(width, height, area, pair.Value).GetHeatMapPoints());
            }

            return new HeatmapsResponse
            {
                NumSources = heatmaps.Count,
                Sources = heatmaps.Keys,
                Heatmaps = heatmaps
            };
        }

        public abstract HeatMap ConvertToHeatmap(int width, int height, BoundingBox area, IEnumerable<DataPoint> hazards);
    }
}
