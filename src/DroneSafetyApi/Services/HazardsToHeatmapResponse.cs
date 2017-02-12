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
        public override Heatmap ConvertToHeatmap(int height, int width, Polygon area, IEnumerable<Hazard> hazards)
        {
            Heatmap map = new Heatmap();
            map.Values = new int[height][];
            for(int i=0; i<height; i++) {
                map.Values[i] = new int[width];
                for(int j=0; j<width; j++)
                {
                    map.Values[i][j] = 0;
                }
            }
            BoundingBox totalArea = area.BoundingBox;
            double rowStart = totalArea.Min.Latitude;
            double colStart = totalArea.Min.Longitude;
            double rowScale = (totalArea.Max.Latitude - rowStart) / ((double) width);
            double colScale = (totalArea.Max.Longitude - colStart) / ((double) height);


            foreach (Hazard hazard in hazards)
            {
                IList<Position> coord = hazard.Area.Rings[0].Positions;
                BoundingBox box = hazard.Area.BoundingBox;

                int startX = (int)(box.Min.Latitude / rowScale);
                int endX = (int)(box.Max.Latitude / rowScale);
                int startY = (int)(box.Min.Longitude / colScale);
                int endY = (int)(box.Max.Longitude / colScale);

                for (int x = startX; x < endX; x++)
                {
                    for (int y = startY; y < endY; y++)
                    {
                        map.Values[y][x] += inHazard(x * rowScale, y * colScale, hazard) ? hazard.Severity : 0;
                    }
                }
            }

            return map;
        }

        public Boolean inHazard(double lat, double lon, Hazard hazard)
        {
            Boolean inside = false;
            IList<Position> coord = hazard.Area.Rings[0].Positions;
            for (int i = 1; i < coord.Count; i++)
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
