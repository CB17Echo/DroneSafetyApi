using DroneSafetyApi.Models;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Services
{

    public class HeatMap
    {
        private Dictionary<Position, int> Map;
        public double StartX { get; set; }
        public double StartY { get; set; }
        public double EndX { get; set; }
        public double EndY { get; set; }
        public double Delta { get; set; }

        public HeatMap(double minX, double maxX, double minY, double maxY, double resolution)
        {
            StartX = minX;
            StartY = minY;
            EndX = maxX;
            EndY = maxY;

            Delta = resolution;

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
            int aX = (int)((pos.Longitude - StartX) / Delta);
            int aY = (int)((pos.Latitude - StartY) / Delta);
            double x = StartX + Delta * aX;
            double y = StartY + Delta * aY;
            return new Position (x, y);
        }
    }
}