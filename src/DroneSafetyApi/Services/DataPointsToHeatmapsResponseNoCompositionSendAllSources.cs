using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Services
{
    public abstract class DataPointsToHeatmapsResponseNoCompositionSendAllSources : IHazardsToHeatmapsResponse
    {
        public HeatmapsResponse ConvertToHeatmapResponse(
            int decimalPlaces,
            BoundingBox area,
            IEnumerable<DataPoint> datapoints)
        {
            Dictionary<string, List<DataPoint>> datapointdictionary = new Dictionary<string, List<DataPoint>>();

            foreach (DataPoint datapoint in datapoints)
            {
                List<DataPoint> list;
                datapointdictionary.TryGetValue(datapoint.DataType, out list);
                if (list == null)
                {
                    list = new List<DataPoint>();
                    datapointdictionary.Add(datapoint.DataType, list);
                }
                list.Add(datapoint);
            }

            Dictionary<String, HeatMap> heatmaps = new Dictionary<string, HeatMap>();

            foreach(KeyValuePair<string, List<DataPoint>> pair in datapointdictionary)
            {
                heatmaps.Add(pair.Key, ConvertToHeatmap(decimalPlaces, area, pair.Value));
            }

            return new HeatmapsResponse
            {
                NumSources = heatmaps.Count,
                Sources = heatmaps.Keys,
                Heatmaps = heatmaps
            };
        }

        public abstract HeatMap ConvertToHeatmap(int decimalPlaces, BoundingBox area, IEnumerable<DataPoint> hazards);
    }
}
