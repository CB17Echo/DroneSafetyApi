using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Models
{
    public class HeatmapResponse
    {
        public int NumSources { get; set; }
        public string[] Sources { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        // Keys in HeatMaps must be exactly those in Sources
        // Values in HeatMaps must be int[Height][Width]
        public Dictionary<string, Heatmap> Heatmaps { get; set; }
    }
}
