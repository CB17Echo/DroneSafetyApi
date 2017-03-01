using System.Collections.Generic;

namespace DroneSafetyApi.Models
{
    /// <summary>
    /// The HeatmapResponse class is a model to hold information served over the api
    /// </summary>
    public class HeatmapsResponse
    {
        /// <summary>
        /// <para>The NumSources property represents the number of different data sources processed</para>
        /// <para>Numsources = Sources.Count = Heatmaps.Count</para>
        /// </summary>
        public int NumSources { get; set; }
        /// <summary>
        /// The Sources property represents the collection of names of the different data sources processed
        /// </summary>
        public IEnumerable<string> Sources { get; set; }
        /// <summary>
        /// <para>The Heatmaps property represents a mapping from a data source to the <see cref="HeatmapPoint"/>s that display it</para>
        /// <para>The string keys of Heatmaps correspond exactly to the elements in sources</para>
        /// </summary>
        public Dictionary<string, IEnumerable<HeatmapPoint>> Heatmaps { get; set; }
    }
}
