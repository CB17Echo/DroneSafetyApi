﻿using DroneSafetyApi.Models;
using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Services
{

    public class HeatMap
    {
        private Dictionary<Position, int> Map;
        private double StartX;
        private double StartY;
        private double EndX;
        private double EndY;
        private double DeltaX;
        private double DeltaY;

        private int DecimalPlacesX;
        private int DecimalPlacesY;

        const int MetresInLatDegree = 110575;

        public HeatMap(double minX, double maxX, double minY, double maxY, int width, int height)
        {
            StartX = minX;
            StartY = minY;
            EndX = maxX;
            EndY = maxY;

            DeltaX = (maxX - minX) / width;
            DeltaY = (maxY - minY) / height;
            DecimalPlacesX = 0;
            while ((int)DeltaX % 10 == 0)
            {
                DecimalPlacesX++;
                DeltaX *= 10;
            }
            DeltaX /= Math.Pow(10, DecimalPlacesX);
            DecimalPlacesX += (int)Math.Log10(width);
            DeltaX = Math.Round(DeltaX, DecimalPlacesX);

            DecimalPlacesY = 0;
            while ((int)DeltaY % 10 == 0)
            {
                DecimalPlacesY++;
                DeltaY *= 10;
            }
            DeltaY /= Math.Pow(10, DecimalPlacesY);
            DecimalPlacesY += (int)Math.Log10(height);
            DeltaY = Math.Round(DeltaY, DecimalPlacesY);

            Map = new Dictionary<Position, int>();
            
        }

        private void AddHazard(double x, double y, int v)
        {
            if(x < StartX || x > EndX || y < StartY || y > EndY) { return; }
            Position pos = new Position(Math.Round(x, DecimalPlacesX), Math.Round(y, DecimalPlacesY));
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
            int aX = (int)((pos.Longitude - StartX) / DeltaX);
            int aY = (int)((pos.Latitude - StartY) / DeltaY);
            double x = Math.Round(StartX + DeltaX * aX, DecimalPlacesX);
            double y = Math.Round(StartY + DeltaY * aY, DecimalPlacesY);
            return new Position (x, y);
        }

        public void ProcessPoint(Point point, int value)
        {
            Position pos = GetNearestPosition(point.Position);
            AddHazard(pos.Longitude, pos.Latitude, value);
        }

        public void ProcessCircle(Point circleCentre, int radius, int value)
        {
            Position center = GetNearestPosition(circleCentre.Position);
            double lon = center.Longitude;
            double lat = center.Latitude;
            AddHazard(lon, lat, value);

            double radiusDeg = MetresToDegrees(radius);

            int radiusStepsX = (int)(radiusDeg / DeltaX);
            int radiusStepsY = (int)(radiusDeg / DeltaY);
            if (radiusStepsX > 0)
            {
                AddHazard(lon - DeltaX, lat, value);
                AddHazard(lon + DeltaX, lat, value);
            }
            if (radiusStepsY > 0)
            {
                AddHazard(lon, lat - DeltaY, value);
                AddHazard(lon, lat + DeltaY, value);
            }
            for (int x = 1; x < radiusStepsX; x++)
                for (int y = 1; y < radiusStepsX; y++)
                {
                    if (x * x + y * y <= radiusStepsX * radiusStepsY)
                    {
                        
                        double deltaLon = x * DeltaX;
                        double deltaLat = y * DeltaY;
                        AddHazard(lon - deltaLon, lat - deltaLat, value);
                        AddHazard(lon - deltaLon, lat + deltaLat, value);
                        AddHazard(lon + deltaLon, lat - deltaLat, value);
                        AddHazard(lon + deltaLon, lat + deltaLat, value);
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
            if (minLong < StartX) { minLong = StartX; }
            if (maxLong > EndX) { maxLong = EndX; }
            if (minLat < StartY) { minLat = StartY; }
            if (maxLat > EndY) { maxLat = EndY; }



            // Calculate grid elements to go through
            Position start = GetNearestPosition(new Position(minLong, minLat));

            for (double x = start.Longitude; x < maxLong; x += DeltaX)
            {
                for (double y = start.Latitude; y < maxLat; y += DeltaY)
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