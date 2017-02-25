using DroneSafetyApi.Models;
using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Services
{

    public class HeatMap
    {
        private Dictionary<Position, int> Map;
        private double MinLon;
        private double MinLat;
        private double MaxLon;
        private double MaxLat;

        private double Resolution;
        


        public const int MetresInLatDegree = 110575;

        public HeatMap(double minX, double maxX, double minY, double maxY, int NumberLonPoints)
        {
            MinLon = minX;
            MinLat = minY;
            MaxLon = maxX;
            MaxLat = maxY;

            Resolution = (MaxLon - MinLon) / NumberLonPoints;

            Map = new Dictionary<Position, int>();
            
        }

        private void AddHazard(double x, double y, int v)
        {
            if(x < MinLon || x > MaxLon || y < MinLat || y > MaxLat) { return; }

            Position pos = GetNearestPosition(new Position(x, y));


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

        public void ProcessPoint(Point point, int value)
        {
            Position pos = GetNearestPosition(point.Position);
            AddHazard(pos.Longitude, pos.Latitude, value);
        }

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
            if (minLong < MinLon) { minLong = MinLon; }
            if (maxLong > MaxLon) { maxLong = MaxLon; }
            if (minLat < MinLat) { minLat = MinLat; }
            if (maxLat > MaxLat) { maxLat = MaxLat; }

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