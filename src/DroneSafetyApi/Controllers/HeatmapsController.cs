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

            double x = query.Area.Min.Latitude + (query.Area.Max.Latitude - query.Area.Min.Latitude) / 2;
            double y = query.Area.Min.Longitude + (query.Area.Max.Longitude - query.Area.Min.Longitude) / 2;

            int radius =100000;

            var intersectionHazards = DataPoints.GetDataPointsInRadius(x, y, radius);
            var heatmapsResponse = DataPointsToHeatmaps.ConvertToHeatmapResponse(4,
                query.Area,
                intersectionHazards);
            return new ObjectResult(heatmapsResponse);
        }
    }
}
