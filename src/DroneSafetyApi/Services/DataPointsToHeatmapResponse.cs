using System;
using System.Collections.Generic;
using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Services
{
    public class DataPointsToHeatmapResponse : DataPointsToHeatmapsResponseNoCompositionSendAllSources
    {

        public override HeatMap ConvertToHeatmap(BoundingBox area, int width, int height, IEnumerable<DataPoint> datapoints)
        {
            // Initialise Heatmap
            HeatMap heatmap = new HeatMap(area.Min.Longitude, area.Max.Longitude, area.Min.Latitude, area.Max.Latitude, width, height);

            foreach (DataPoint datapoint in datapoints)
            {
                switch (datapoint.Shape)
                {
                    case "Point":
                        Point point = (Point)datapoint.Location;
                        heatmap.ProcessPoint(point, datapoint.Severity);
                        break;
                    case "Circle":
                        Point centre = (Point)datapoint.Location;
                        CircleDataPoint circle = (CircleDataPoint)datapoint;
                        heatmap.ProcessCircle(centre, circle.Radius, datapoint.Severity);
                        break;
                    case "Polygon":
                        Polygon polygon = (Polygon)datapoint.Location;
                        heatmap.ProcessPolygon(polygon, datapoint.Severity);
                        break;
                    default:
                        break;
                }
            }

            return heatmap;
        }
    }
}
