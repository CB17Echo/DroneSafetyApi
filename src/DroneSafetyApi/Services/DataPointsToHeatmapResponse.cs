using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Services
{
    public class DataPointsToHeatmapResponse : DataPointsToHeatmapsResponseNoCompositionSendAllSources
    {

        const int MetresInLatDegree = 110575;

        public override HeatMap ConvertToHeatmap(int decimalPlaces, BoundingBox area, IEnumerable<DataPoint> datapoints)
        {
            // Initialise Heatmap
            HeatMap heatmap = new HeatMap(area.Min.Latitude, area.Max.Latitude, area.Min.Longitude, area.Max.Longitude, decimalPlaces);

            foreach (DataPoint datapoint in datapoints)
            {
                switch (datapoint.Shape)
                {
                    case "Point":
                        Point point = (Point)datapoint.Location;
                        ProcessPoint(point, heatmap, datapoint.Severity);
                        break;
                    /*case "Cirlce":
                        Point centre = (Point)datapoint.Location;
                        ProcessCircle(centre, radius, heatmap, datapoint.Severity);
                        break;*/
                    case "Polygon":
                        Polygon polygon = (Polygon)datapoint.Location;
                        ProcessPolygon(polygon, heatmap, datapoint.Severity);
                        break;
                    default:
                        break;
                }
            }

            return heatmap;
        }

        private void ProcessPoint(Point point, HeatMap heatmap, int value)
        {
            heatmap.AddHazard(point.Position.Latitude, point.Position.Latitude, value);
        }

        private void ProcessCircle(Point centre, int radius, HeatMap heatmap, int value)
        {
            double lat = centre.Position.Latitude;
            double lon = centre.Position.Longitude;

            double resolutionDeg = heatmap.GetResolution();
            double radiusDeg = MetresToDegrees(radius);

            int radiusSteps = (int)(radiusDeg / resolutionDeg);

            for (int x = 0; x < radiusSteps; x++)
                for (int y = 0; y < radiusSteps; y++)
                {
                    if (x * x + y * y <= radiusSteps * radiusSteps)
                    {
                        double deltaLat = x * resolutionDeg;
                        double deltaLon = y * resolutionDeg;

                        heatmap.AddHazard(lat - deltaLat, lon - deltaLon, value);
                        heatmap.AddHazard(lat - deltaLat, lon + deltaLon, value);
                        heatmap.AddHazard(lat + deltaLat, lon - deltaLon, value);
                        heatmap.AddHazard(lat + deltaLat, lon + deltaLon, value);
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
            if (minLat < heatmap.mStartX) { minLat = heatmap.mStartX; }
            if (maxLat > heatmap.mEndX) { maxLat = heatmap.mEndX; }
            if (minLong < heatmap.mStartY) { minLong = heatmap.mStartY; }
            if (maxLong > heatmap.mEndY) { maxLong = heatmap.mEndY; }


            // Calculate grid elements to go through
            int[] start = heatmap.GPSToIndex(minLat, minLong);
            int[] end = heatmap.GPSToIndex(maxLat, maxLong);

            for (int x = start[0]; x < end[0]; x++)
            {
                for (int y = start[1]; y < end[1]; y++)
                {
                    double[] point = heatmap.indexToGPS(x, y);
                    if (inHazard(point[0], point[1], polygon))
                    {
                        heatmap.AddHazard(point[0], point[1], value);
                    }
                }
            }
        }

        public Boolean inHazard(double lat, double lon, Polygon polygon)
        {
            Boolean inside = false;
            IList<Position> coord = polygon.Rings[0].Positions;
            for (int i = 1; i < coord.Count ; i++)
            {
                if (lat == coord[i].Latitude && lon == coord[i].Longitude) return true;
                if (((coord[i].Longitude > lon) != (coord[i-1].Longitude > lon)) &&
                    (lat < (coord[i-1].Latitude - coord[i].Latitude) * (lon - coord[i].Longitude) /
                    (coord[i-1].Longitude - coord[i].Longitude) + coord[i].Latitude)) { inside = !inside; }
            }
            return inside;
        }

    }
}
