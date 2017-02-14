using DroneSafetyApi.Models;
using DroneSafetyApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Controllers
{
    [Route("api/[controller]")]
    public class HeatmapsController : Controller
    {
        public IDataPointRepository Hazards { get; set; }
        public IDataPointsToHeatmapsResponse HazardsToHeatmaps { get; set; }
        public HeatmapsController(
            IDataPointRepository hazards,
            IDataPointsToHeatmapsResponse hazardsToHeatmaps)
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
            var intersectionHazards = Hazards.GetHazardsOverlappingWith(query.Area);
            var heatmapsResponse = HazardsToHeatmaps.ConvertToHeatmapResponse(
                query.Height,
                query.Width,
                query.Area,
                intersectionHazards);
            return new ObjectResult(heatmapsResponse);
        }
    }
}
