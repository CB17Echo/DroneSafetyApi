﻿using System;
using System.Collections.Generic;
using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Services
{
    /// <summary>
    /// The HazardsToHeatmapResponse class is a service to generate <see cref="IHeatmap"/>s and map
    /// a collection of <see cref="Hazard"/>s onto them
    /// </summary>
    public class HazardsToHeatmapResponse : HazardsToHeatmapsResponseNoCompositionSendAllSources
    {
        /// <summary>
        /// <see cref="HazardsToHeatmapsResponseNoCompositionSendAllSources.HazardsToHeatmapsResponseNoCompositionSendAllSources(IHeatmapFactory)"/>
        /// </summary>
        public HazardsToHeatmapResponse(IHeatmapFactory heatmapsFactory) : base(heatmapsFactory) { }

        /// <summary>
        /// <see cref="HazardsToHeatmapsResponseNoCompositionSendAllSources.ConvertToHeatmap(Bounds, int, IEnumerable{Hazard})"/>
        /// </summary>
        public override IHeatmap ConvertToHeatmap(Bounds area, int numberLonPoints, IEnumerable<Hazard> hazards)
        {
            // Initialise Heatmap
            IHeatmap heatmap = HeatmapFactory.CreateHeatmap(area, numberLonPoints);

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
                        if (circle.DataType == "Bus")
                            circle.Severity *= 10;
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
