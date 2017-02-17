using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Models
{
    public class HeatmapPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public int Value { get; set; }
    }
}
