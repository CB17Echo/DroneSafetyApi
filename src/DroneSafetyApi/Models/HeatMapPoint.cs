using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Models
{
    public class HeatMapPoint
    {
        public double x { get; set; }
        public double y { get; set; }

        public int value { get; set; }

    }
}
