using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Services
{
    public interface IHeatmap
    {
        IEnumerable<HeatmapPoint> GetHeatmapPoints();
        void ProcessPoint(Point point, int value);
        void ProcessCircle(Point circleCentre, int radius, int value);
        void ProcessPolygon(Polygon polygon, int value);
    }
}
