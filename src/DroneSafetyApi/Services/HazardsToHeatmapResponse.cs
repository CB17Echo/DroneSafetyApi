using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Services
{
    public class HazardsToHeatmapResponse : HazardsToHeatmapsResponseNoCompositionSendAllSources
    {
        public override Heatmap ConvertToHeatmap(int height, int width, BoundingBox area, IEnumerable<Hazard> hazards)
        {
            // Initialise Heatmap
            Heatmap heatmap = new Heatmap();
            heatmap.Values = new int[width,height];

            // Calculate scaling between grid and lat long coordinates
            double xScale = (area.Max.Latitude - area.Min.Latitude) / width;
            double yScale = (area.Max.Longitude - area.Min.Longitude) / height;

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
                int xStart = (int)((minLat - area.Min.Latitude) / xScale);
                int xRange = (int)((maxLat - minLat) / xScale);
                int yStart = (int)((minLong - area.Min.Longitude) / yScale);
                int yRange = (int)((maxLong - minLong) / yScale);

                for(int x = 0; x < xRange; x++)
                {
                    for(int y = 0; y < yRange; y++)
                    {
                        heatmap.Values[xStart+x, height - 1 - (yStart + y)] += (inHazard(minLat + (x * xScale),
                            minLong + (y * yScale), hazard)) ? hazard.Severity : 0;
                    }
                }
            }

            return heatmap;
        }

        public Boolean inHazard(double lat, double lon, Hazard hazard)
        {
            Boolean inside = false;
            IList<Position> coord = hazard.Area.Rings[0].Positions;
            for (int i = 1; i < coord.Count ; i++)
            {
                if (lat == coord[i].Latitude && lon == coord[i].Longitude) return true;
                if (((coord[i].Longitude > lon) != (coord[i-1].Longitude > lon)) &&
                    (lat < (coord[i-1].Latitude - coord[i].Latitude) * (lon - coord[i].Longitude) /
                    (coord[i-1].Longitude - coord[i].Longitude) + coord[i].Latitude)) { inside = !inside; }
            }
            return inside;
        }


        public override HeatmapsResponse ConvertToHeatmapResponse(int height, int width, Polygon area, Dictionary<string, IEnumerable<Hazard>> hazards)
        {
            HeatmapsResponse response = new HeatmapsResponse();
            response.Height = height;
            response.Width = width;
            response.NumSources = hazards.Count;
            response.Sources = hazards.Keys;
            foreach(IEnumerable<Hazard> source in hazards.Values)
            {
                con
            }

            return response;
        }

    }
}
