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
        public IHazardsRepository Hazards { get; set; }
        public HazardsToHeatmapsResponseStrategy HazardsToHeatmaps { get; set; }
        public HeatmapsController(
            IHazardsRepository hazards,
            HazardsToHeatmapsResponseStrategy hazardsToHeatmaps)
        {
            Hazards = hazards;
            HazardsToHeatmaps = hazardsToHeatmaps;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]HeatmapsQuery query)
        {
            if (BadQuery(query))
            {
                return new BadRequestResult();
            }
            var intersectionHazards = Hazards.GetHazardsIntersectingWith(query.Area);
            var heatmapsResponse = HazardsToHeatmaps.ConvertToHeatmapResponse(
                query.Height,
                query.Width,
                query.Area,
                intersectionHazards);
            return new ObjectResult(heatmapsResponse);
        }

        private bool BadQuery(HeatmapsQuery query)
        {
            return (query.Height <= 0)
                || (query.Width <= 0);
        }
    }
}
