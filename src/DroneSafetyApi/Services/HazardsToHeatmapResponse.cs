using System;
using System.Collections.Generic;
using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Services
{
    public class HazardsToHeatmapResponse : HazardsToHeatmapsResponseNoCompositionSendAllSources
    {

        public override HeatMap ConvertToHeatmap(Bounds area, int numberLonPoints, IEnumerable<Hazard> hazards)
        {
            // Initialise Heatmap
            HeatMap heatmap = new HeatMap(area, numberLonPoints);

            foreach (Hazard hazard in hazards)
            {
                switch (hazard.Shape)
                {
                    case "Point":
                        PointHazard point = (PointHazard)hazard;
                        heatmap.ProcessPoint(point.Location, hazard.Severity);
                        break;
                    case "Circle":
                        CircularHazard circle = (CircularHazard)hazard;
                        heatmap.ProcessCircle(circle.Location, circle.Radius, hazard.Severity);
                        break;
                    case "Polygon":
                        PolygonalHazard polygon = (PolygonalHazard)hazard;
                        heatmap.ProcessPolygon(polygon.Location, hazard.Severity);
                        break;
                    default:
                        break;
                }
            }

            return heatmap;
        }
    }
}
