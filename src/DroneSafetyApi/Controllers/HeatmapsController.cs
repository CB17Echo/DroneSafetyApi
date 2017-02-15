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
        public IDataPointRepository DataPoints { get; set; }
        public IDataPointsToHeatmapsResponse DataPointsToHeatmaps { get; set; }
        public HeatmapsController(
            IDataPointRepository hazards,
            IDataPointsToHeatmapsResponse hazardsToHeatmaps)
        {
            DataPoints = hazards;
            DataPointsToHeatmaps = hazardsToHeatmaps;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]HeatmapsQuery query)
        {
            if (query.Bad)
            {
                return new BadRequestResult();
            }
            var intersectionHazards = DataPoints.GetHazardsOverlappingWith(query.Area);
            var heatmapsResponse = DataPointsToHeatmaps.ConvertToHeatmapResponse(
                query.Height,
                query.Width,
                query.Area,
                intersectionHazards);
            return new ObjectResult(heatmapsResponse);
        }
    }
}
