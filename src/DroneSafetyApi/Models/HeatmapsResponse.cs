using System.Collections.Generic;

namespace DroneSafetyApi.Models
{
    /// <summary>
    /// The HeatmapResponse class is a model to hold information that is being sent to the web application
    /// </summary>
    public class HeatmapsResponse
    {
        /// <summary>
        /// The NumSources property represents the number of different data sources processed
        /// </summary>
        public int NumSources { get; set; }
        /// <summary>
        /// The Sources property represents the collection of names of the different data sources processed
        /// </summary>
        public IEnumerable<string> Sources { get; set; }
        /// <summary>
        /// The Heatmaps property represents a mapping from a data source to the <see cref="HeatmapPoint"/>s that display it
        /// </summary>
        public Dictionary<string, IEnumerable<HeatmapPoint>> Heatmaps { get; set; }
    }
}
