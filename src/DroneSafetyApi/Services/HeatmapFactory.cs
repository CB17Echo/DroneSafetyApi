using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroneSafetyApi.Models;

namespace DroneSafetyApi.Services
{
    public class HeatmapFactory : IHeatmapFactory
    {
        public IHeatmap CreateHeatmap(Bounds area, int numberLonPoints)
        {
            return new Heatmap(area, numberLonPoints);
        }
    }
}
