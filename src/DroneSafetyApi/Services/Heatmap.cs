using DroneSafetyApi.Models;
using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Services
{
    /// <summary>
    /// The Heatmap class is a service used to calculate the points of <see cref="Hazard"/>s on a heatmap
    /// </summary>
    public class Heatmap : IHeatmap
    {
        private Dictionary<Position, int> Map;
        private Bounds Area;
        private double Resolution;

        /// <summary>
        /// The MetersInLatDegree constant represents the numberr of meters in a single Latitude degree
        /// </summary>
        public const int MetresInLatDegree = 110575;

        /// <summary>
        /// Creates a new instance of the <see cref="Heatmap"/> class 
        /// </summary>
        /// <param name="area">The bounding box that the new <see cref="Heatmap"/> instance covers</param>
        /// <param name="numberLonPoints">The number of points along the Longitude axis in the new <see cref="Heatmap"/> instance</param>
        public Heatmap(Bounds area, int numberLonPoints)
        {
            Area = area;
            Resolution = (Area.Max.Longitude - Area.Min.Longitude) / numberLonPoints;

            Map = new Dictionary<Position, int>();
            
        }

        private void AddHazard(double x, double y, int v)
        {
            if (x < Area.Min.Longitude || x > Area.Max.Longitude || y < Area.Min.Latitude || y > Area.Max.Latitude) { return; }
            Position pos = GetNearestPosition(new Position(x, y));
            if (Map.ContainsKey(pos))
            {
                Map[pos] += v;
            } else
            {
                Map.Add(pos, v);
            }
        }

        /// <summary>
        /// <see cref="IHeatmap.GetHeatmapPoints()"/> 
        /// </summary>
        public IEnumerable<HeatmapPoint> GetHeatmapPoints()
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

        private Position GetNearestPosition(Position pos)
        {
            decimal halfRes = ((decimal)Resolution) / 2;

            decimal xRem = ((decimal)pos.Longitude) % ((decimal)Resolution);
            decimal yRem = ((decimal)pos.Latitude) % ((decimal)Resolution);

            if (xRem > halfRes) { xRem *= -1; }
            if (yRem > halfRes) { yRem *= -1; }

            int dx = (int)((((decimal)pos.Longitude) - xRem) / (decimal) Resolution);
            int dy = (int)((((decimal)pos.Latitude) - yRem) / (decimal) Resolution);
            return new Position(dx * Resolution, dy * Resolution);
        }

        /// <summary>
        /// <see cref="IHeatmap.ProcessPoint(Point, int)"/> 
        /// </summary>
        public void ProcessPoint(Point point, int value)
        {
            Position pos = GetNearestPosition(point.Position);
            AddHazard(pos.Longitude, pos.Latitude, value);
        }

        /// <summary>
        /// <see cref="IHeatmap.ProcessCircle(Point, int, int)"/> 
        /// </summary>
        public void ProcessCircle(Point circleCentre, int radius, int value)
        {
            double radiusDeg = MetresToDegrees(radius);

            for (double longitude = circleCentre.Position.Longitude - radiusDeg; 
                 longitude <= circleCentre.Position.Longitude + radiusDeg;
                 longitude += Resolution)
            {
                for (double latitude = circleCentre.Position.Latitude - radiusDeg;
                     latitude <= circleCentre.Position.Latitude + radiusDeg;
                     latitude += Resolution)
                {
                    double deltaLat = latitude - circleCentre.Position.Latitude;
                    double deltaLon = longitude - circleCentre.Position.Longitude;
                    if (deltaLat * deltaLat + deltaLon * deltaLon <= radiusDeg * radiusDeg)
                        AddHazard(longitude, latitude, value);
                }
            }
        }

        private double MetresToDegrees(double metres)
        {
            return metres / MetresInLatDegree;
        }

        /// <summary>
        /// <see cref="IHeatmap.ProcessPolygon(Polygon, int)"/> 
        /// </summary>
        public void ProcessPolygon(Polygon polygon, int value)
        {
            IList<Position> coord = polygon.Rings[0].Positions;

            // Calculate bounding box of each hazard polygon
            double minLong = coord[0].Longitude;
            double maxLong = coord[0].Longitude;
            double minLat = coord[0].Latitude;
            double maxLat = coord[0].Latitude;
            for (int i = 1; i < coord.Count - 1; i++)
            {
                if (coord[i].Latitude < minLat) { minLat = coord[i].Latitude; }
                else if (coord[i].Latitude > maxLat) { maxLat = coord[i].Latitude; }
                if (coord[i].Longitude < minLong) { minLong = coord[i].Longitude; }
                else if (coord[i].Longitude > maxLong) { maxLong = coord[i].Longitude; }
            }
            if (minLong < Area.Min.Longitude) { minLong = Area.Min.Longitude; }
            if (maxLong > Area.Max.Longitude) { maxLong = Area.Max.Longitude; }
            if (minLat < Area.Min.Latitude) { minLat = Area.Min.Latitude; }
            if (maxLat > Area.Max.Latitude) { maxLat = Area.Max.Latitude; }

            // Calculate grid elements to go through
            Position start = GetNearestPosition(new Position(minLong, minLat));

            for (double x = start.Longitude; x < maxLong; x += Resolution)
            {
                for (double y = start.Latitude; y < maxLat; y += Resolution)
                {
                    if (InHazard(x, y, polygon))
                    {
                        AddHazard(x, y, value);
                    }
                }
            }
        }

        private bool InHazard(double lon, double lat, Polygon polygon)
        {
            bool inside = false;
            IList<Position> coord = polygon.Rings[0].Positions;
            for (int i = 1; i < coord.Count; i++)
            {
                if (lon == coord[i].Longitude && lat == coord[i].Latitude) return true;
                if (((coord[i].Latitude > lat) != (coord[i - 1].Latitude > lat)) &&
                    (lon < (coord[i - 1].Longitude - coord[i].Longitude) * (lat - coord[i].Latitude) /
                    (coord[i - 1].Latitude - coord[i].Latitude) + coord[i].Longitude)) { inside = !inside; }
            }
            return inside;
        }
    }
}