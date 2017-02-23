﻿using System;
using System.Collections.Generic;
using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Services
{
    public class HazardsToHeatmapResponse : HazardsToHeatmapsResponseNoCompositionSendAllSources
    {

        const int MetresInLatDegree = 110575;

        public override HeatMap ConvertToHeatmap(double resolution, BoundingBox area, IEnumerable<Hazard> hazards)
        {
            // Initialise Heatmap
            HeatMap heatmap = new HeatMap(area.Min.Longitude, area.Max.Longitude, area.Min.Latitude, area.Max.Latitude, resolution);

            foreach (Hazard hazard in hazards)
            {
                switch (hazard.Shape)
                {
                    case "Point":
                        Point point = (Point)hazard.Location;
                        ProcessPoint(point, heatmap, hazard.Severity);
                        break;
                    case "Cirlce":
                        Point centre = (Point)hazard.Location;
                        CircularHazard circle = (CircularHazard)hazard;
                        ProcessCircle(centre, circle.Radius, heatmap, hazard.Severity);
                        break;
                    case "Polygon":
                        Polygon polygon = (Polygon)hazard.Location;
                        ProcessPolygon(polygon, heatmap, hazard.Severity);
                        break;
                    default:
                        break;
                }
            }

            return heatmap;
        }

        private void ProcessPoint(Point point, HeatMap heatmap, int value)
        {
            Position pos = heatmap.GetNearestPosition(point.Position);
            heatmap.AddHazard(pos.Longitude, pos.Latitude, value);
        }

        private void ProcessCircle(Point circleCentre, int radius, HeatMap heatmap, int value)
        {
            Position center = heatmap.GetNearestPosition(circleCentre.Position);
            double lon = center.Longitude;
            double lat = center.Latitude;

            double resolutionDeg = heatmap.Delta;
            double radiusDeg = MetresToDegrees(radius);

            int radiusSteps = (int)(radiusDeg / resolutionDeg);

            for (int x = 0; x < radiusSteps; x++)
                for (int y = 0; y < radiusSteps; y++)
                {
                    if (x * x + y * y <= radiusSteps * radiusSteps)
                    {
                        double deltaLon = x * resolutionDeg;
                        double deltaLat = y * resolutionDeg;

                        heatmap.AddHazard(lon - deltaLon, lat - deltaLat, value);
                        heatmap.AddHazard(lon - deltaLon, lat + deltaLat, value);
                        heatmap.AddHazard(lon + deltaLon, lat - deltaLat, value);
                        heatmap.AddHazard(lon + deltaLon, lat + deltaLat, value);
                    }
                }
        }

        private double MetresToDegrees(double metres)
        {
            return metres / MetresInLatDegree;
        }

        private void ProcessPolygon(Polygon polygon, HeatMap heatmap, int value)
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
            if (minLong < heatmap.StartX) { minLong = heatmap.StartX; }
            if (maxLong > heatmap.EndX) { maxLong = heatmap.EndX; }
            if (minLat < heatmap.StartY) { minLat = heatmap.StartY; }
            if (maxLat > heatmap.EndY) { maxLat = heatmap.EndY; }
            


            // Calculate grid elements to go through
            Position start = heatmap.GetNearestPosition(new Position(minLong, minLat));
            Position end = heatmap.GetNearestPosition(new Position(maxLong, maxLat));

            for (double x = start.Longitude; x < end.Longitude; x += heatmap.Delta)
            {
                for (double y = start.Latitude; y < end.Latitude; y += heatmap.Delta)
                {
                    if (InHazard(x, y, polygon))
                    {
                        heatmap.AddHazard(x, y, value);
                    }
                }
            }
        }

        private bool InHazard(double lon, double lat, Polygon polygon)
        {
            bool inside = false;
            IList<Position> coord = polygon.Rings[0].Positions;
            for (int i = 1; i < coord.Count ; i++)
            {
                if (lon == coord[i].Longitude && lat == coord[i].Latitude) return true;
                if (((coord[i].Latitude > lat) != (coord[i-1].Latitude > lat)) &&
                    (lon < (coord[i-1].Longitude - coord[i].Longitude) * (lat - coord[i].Latitude) /
                    (coord[i-1].Latitude - coord[i].Latitude) + coord[i].Longitude)) { inside = !inside; }
            }
            return inside;
        }

    }
}