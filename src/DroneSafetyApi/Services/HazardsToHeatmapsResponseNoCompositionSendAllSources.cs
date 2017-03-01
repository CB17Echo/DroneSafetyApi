using System.Collections.Generic;
using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Services
{
    /// <summary>
    /// <para>The HazardsToHeatmapsResponseNoCompositionSendAllSources class is a service to generate <see cref="HeatmapsResponse"/>s
    /// from a collection of <see cref="Hazard"/>s </para>
    /// <para>This class creates a heatmap for each different data source but does not combine the heatmaps</para>
    /// </summary>
    public abstract class HazardsToHeatmapsResponseNoCompositionSendAllSources : IHazardsToHeatmapsResponse
    {
        /// <summary>
        /// The HeatmapFactory property is a <see cref="IHeatmapFactory"/> use to generate new <see cref="IHeatmap"/>s 
        /// </summary>
        public IHeatmapFactory HeatmapFactory { get; set; }
        /// <summary>
        /// Creates a new instance of the <see cref="HazardsToHeatmapsResponseNoCompositionSendAllSources"/> class 
        /// </summary>
        /// <param name="heatmapFactory"> <see cref="IHeatmapFactory"/> used to generate <see cref="IHeatmap"/>s in the instance</param>
        public HazardsToHeatmapsResponseNoCompositionSendAllSources(IHeatmapFactory heatmapFactory)
        {
            HeatmapFactory = heatmapFactory;
        }

        /// <summary>
        /// <see cref="IHazardsToHeatmapsResponse.ConvertToHeatmapResponse(Bounds, int, IEnumerable{Hazard})"/> 
        /// </summary>
        public HeatmapsResponse ConvertToHeatmapResponse(Bounds area, int numberLonPoints, IEnumerable<Hazard> hazards)
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
                heatmaps.Add(pair.Key, ConvertToHeatmap(area, numberLonPoints, pair.Value).GetHeatmapPoints());
            }

            return new HeatmapsResponse
            {
                NumSources = heatmaps.Count,
                Sources = heatmaps.Keys,
                Heatmaps = heatmaps
            };
        }

        /// <summary>
        /// The ConvertToHeatmap method creates a new <see cref="IHeatmap"/> and maps the <see cref="Hazard"/>s onto it
        /// </summary>
        /// <param name="area">The bounding box that the new <see cref="IHeatmap"/> instance covers</param>
        /// <param name="numberLonPoints"></param>
        /// <param name="hazards">The number of points along the Longitude axis in the new <see cref="IHeatmap"/> instance</param>
        /// <returns>A <see cref="IHeatmap"/> with all hazards mapped onto it</returns>
        public abstract IHeatmap ConvertToHeatmap(Bounds area, int numberLonPoints, IEnumerable<Hazard> hazards);
    }
}
