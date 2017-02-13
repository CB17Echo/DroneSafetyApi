using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Services
{
    public class HazardsToHeatmapResponse : HazardsToHeatmapResponseStrategy
    {
        public override Heatmap ConvertToHeatmap(int height, int width, BoundingBox area, IEnumerable<Hazard> hazards)
        {
            // Initialise Heatmap
            Heatmap heatmap = new Heatmap();
            heatmap.Values = new int[height][];
            for(int i = 0; i < height; i++)
            {
                heatmap.Values[i] = new int[width];
                for(int j = 0; j < width; j++)
                {
                    heatmap.Values[i][j] = 0;
                }
            }

            // Calculate scaling between grid and lat long coordinates
            double xScale = (area.Max.Longitude - area.Min.Longitude) / width;
            double yScale = (area.Max.Latitude - area.Min.Latitude) / height;

            foreach (Hazard hazard in hazards)
            {
                IList<Position> coord = hazard.Area.Rings[0].Positions;

                // Calculate bounding box of each hazard polygon
                double minLong = coord[0].Longitude;
                double maxLong = coord[0].Longitude;
                double minLat = coord[0].Latitude;
                double maxLat = coord[0].Latitude;
                for (int i = 1; i < coord.Count - 1; i++)
                {
                    if (coord[i].Longitude < minLong) { minLong = coord[i].Longitude; }
                    else if (coord[i].Longitude > maxLong) { maxLong = coord[i].Longitude; }
                    if (coord[i].Latitude < minLat) { minLat = coord[i].Latitude; }
                    else if (coord[i].Latitude > maxLat) { maxLat = coord[i].Latitude; }
                }
                if (minLong < area.Min.Longitude) { minLong = area.Min.Longitude; }
                if (maxLong > area.Max.Longitude) { maxLong = area.Max.Longitude; }
                if (minLat < area.Min.Latitude) { minLat = area.Min.Latitude; }
                if (maxLat > area.Max.Latitude) { maxLat = area.Max.Latitude; }


                // Calculate grid elements to go through
                int xStart = (int)((minLong - area.Min.Longitude) / xScale);
                int xRange = (int)((maxLong - minLong) / xScale);
                int yStart = (int)((minLat - area.Min.Latitude) / yScale);
                int yRange = (int)((maxLat - minLat) / yScale);

                for(int x = 0; x < xRange; x++)
                {
                    for(int y = 0; y < yRange; y++)
                    {
                        heatmap.Values[height - 1 - (yStart + y)][xStart + x] += (inHazard(minLong + (x * xScale),
                            minLat + (y * yScale), hazard)) ? hazard.Severity : 0;
                    }
                }
            }

            return heatmap;
        }

        public Boolean inHazard(double lon, double lat, Hazard hazard)
        {
            Boolean inside = false;
            IList<Position> coord = hazard.Area.Rings[0].Positions;
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
