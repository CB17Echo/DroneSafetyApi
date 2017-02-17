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
        public Dictionary<string, IEnumerable<HeatmapPoint>> Heatmaps { get; set; }
    }
}
