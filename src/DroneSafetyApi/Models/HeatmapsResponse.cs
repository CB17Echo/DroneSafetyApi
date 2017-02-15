using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Models
{
    public class HeatmapsResponse
    {
        public int NumSources { get; set; }
        public IEnumerable<string> Sources { get; set; }
        // Keys in HeatMaps must be exactly those in Sources
        // Values in HeatMaps must be int[Height][Width]
        public Dictionary<string, IEnumerable<HeatMapPoint>> Heatmaps { get; set; }
    }
}
