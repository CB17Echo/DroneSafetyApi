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
            query.CalculateRadius();
            var intersectionHazards = Hazards.GetHazardsInRadius(query.Centre, query.Radius, query.Time);
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
