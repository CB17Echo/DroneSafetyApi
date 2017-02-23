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
            // TODO: Decide optimal radius value, and put value/computation in appropriate place.
            var intersectionHazards = DataPoints.GetDataPointsInRadius(query.Centre, 100000);
            var heatmapsResponse = DataPointsToHeatmaps.ConvertToHeatmapResponse(
                query.Area,
                query.Width,
                query.Height,
                intersectionHazards
                );
            return new ObjectResult(heatmapsResponse);
        }
    }
}
