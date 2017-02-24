using System.Collections.Generic;
using DroneSafetyApi.Models;
using DroneSafetyApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using DroneSafetyApi.Data;
using System.Linq;

namespace DroneSafetyApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class HeatmapsController : Controller
    {
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
            // TODO: Decide optimal radius value, and put value/computation in appropriate place.
            var intersectionHazards = ((IEnumerable<Hazard>)Hazards.GetHazardsInRadius<CircularHazard>(query.Centre, 100000, "Circle"))
                                        .Concat(Hazards.GetHazardsInRadius<PolygonalHazard>(query.Centre, 100000, "Polygon"))
                                        .Concat(Hazards.GetHazardsInRadius<PointHazard>(query.Centre, 100000, "Point"));
            var heatmapsResponse = HazardsToHeatmaps.ConvertToHeatmapResponse(
                query.Resolution,
                query.Area,
                intersectionHazards
                );
            return new ObjectResult(heatmapsResponse);
        }
    }
}
