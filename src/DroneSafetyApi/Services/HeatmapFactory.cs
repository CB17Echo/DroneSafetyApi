using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroneSafetyApi.Models;

namespace DroneSafetyApi.Services
{
    /// <summary>
    /// The HeatmapFactory class is a service for generating new <see cref="IHeatmap"/>s
    /// </summary>
    public class HeatmapFactory : IHeatmapFactory
    {
        /// <summary>
        /// <see cref="IHeatmapFactory.CreateHeatmap(Bounds, int)"/> 
        /// </summary>
        public IHeatmap CreateHeatmap(Bounds area, int numberLonPoints)
        {
            return new Heatmap(area, numberLonPoints);
        }
    }
}
