using DroneSafetyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Services
{
    public interface IHeatmapFactory
    {
        IHeatmap CreateHeatmap(Bounds area, int numberLonPoints);
    }
}
