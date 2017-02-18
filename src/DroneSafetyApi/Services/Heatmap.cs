using DroneSafetyApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Spatial;

// TODO: Change namespace
namespace DroneSafetyApi.Services
{

    public class HeatMap
    {
        private Dictionary<Position, int> Map;
        public double StartX { get; set; }
        public double EndX { get; set; }
        public double StartY { get; set; }
        public double EndY { get; set; }
        public double DeltaX { get; set; }
        public double DeltaY { get; set; }

        public HeatMap(double minX, double maxX, double minY, double maxY, int width, int height)
        {
            StartX = minX;
            EndX = maxX;
            StartY = minY;
            EndY = maxY;

            DeltaX = (maxX - minX) / width;
            DeltaY = (maxY- minY) / height;

            Map = new Dictionary<Position, int>();
            
        }

        public void AddHazard(double x, double y, int v)
        {
            Position pos = new Position(x, y);
            if (Map.ContainsKey(pos))
            {
                Map[pos] += v;
            } else
            {
                Map.Add(pos, v);
            }
        }

        public IEnumerable<HeatmapPoint> GetHeatMapPoints()
        {
            List<HeatmapPoint> list = new List<HeatmapPoint>();
            foreach(KeyValuePair<Position,int> pair in Map)
            {
                HeatmapPoint point = new HeatmapPoint()
                {
                    X = pair.Key.Longitude,
                    Y = pair.Key.Latitude,
                    Value = pair.Value
                };
                list.Add(point);
            }
            return list;
        }

        public Position GetNearestPosition(Position pos)
        {
            int aX = (int)((pos.Longitude - StartX) / DeltaX);
            double x = StartX + DeltaX * aX;
            int aY = (int)((pos.Latitude - StartY) / DeltaY);
            double y = StartY + DeltaY * aY;
            return new Position (x, y);
        }
    }
}