using System;
using System.Collections.Generic;
using System.Linq;
using DroneSafetyApi.Models;
using DroneSafetyApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using DroneSafetyApi.Data;

namespace DroneSafetyApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class HeatmapsController : Controller
    {
        const int MetresInLatDegree = 110575;

        public IHazardRepository Hazards { get; set; }
        public IHazardsToHeatmapsResponse HazardsToHeatmaps { get; set; }
        public HeatmapsController(
            IHazardRepository hazards,
            IHazardsToHeatmapsResponse hazardsToHeatmaps)
        {
            Hazards = hazards;
            HazardsToHeatmaps = hazardsToHeatmaps;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]HeatmapsQuery query)
        {
            if (query.Bad)
            {
                return new BadRequestResult();
            }
            var radius = (int) (Math.Sqrt(((query.Area.Max.Latitude - query.Area.Min.Latitude) *
                (query.Area.Max.Latitude - query.Area.Min.Latitude)) +
                ((query.Area.Max.Longitude - query.Area.Min.Longitude) *
                (query.Area.Max.Longitude - query.Area.Min.Longitude)))) * MetresInLatDegree;
            var intersectionHazards = ((IEnumerable<Hazard>)
                Hazards.GetHazardsInRadius<CircularHazard>(query.Centre, radius, "Circle", query.Time))
                .Concat(Hazards.GetHazardsInRadius<PolygonalHazard>(query.Centre, radius, "Polygon", query.Time))
                .Concat(Hazards.GetHazardsInRadius<PointHazard>(query.Centre, radius, "Point", query.Time));
            var heatmapsResponse = HazardsToHeatmaps.ConvertToHeatmapResponse(
                query.Area,
                query.Width,
                query.Height,
                intersectionHazards
                );
            return new ObjectResult(heatmapsResponse);
        }
    }
}
