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
            // TODO: Decide optimal radius value, and put value/computation in appropriate place.
            var intersectionHazards = Hazards.GetHazardsInRadius(query.Centre, 100000);
            var heatmapsResponse = HazardsToHeatmaps.ConvertToHeatmapResponse(
                query.Resolution,
                query.Area,
                intersectionHazards
                );
            return new ObjectResult(heatmapsResponse);
        }
    }
}
