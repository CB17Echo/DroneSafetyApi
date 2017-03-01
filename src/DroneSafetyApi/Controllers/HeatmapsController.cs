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

        public IFutureTimeToHeatmapResponse FutureTimeToHeatmap { get; set; }
        public HeatmapsController(
            IHazardRepository hazards,
            IHazardsToHeatmapsResponse hazardsToHeatmaps,
            IFutureTimeToHeatmapResponse futureTimeToHeatmap)
        {
            Hazards = hazards;
            HazardsToHeatmaps = hazardsToHeatmaps;
            FutureTimeToHeatmap = futureTimeToHeatmap;
        }

        [HttpGet]
        public IActionResult GetHeatmaps([FromQuery]HeatmapsQuery query)
        {   
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            if (query.Time.CompareTo(DateTime.Now) <= 0)
            {
                var intersectionHazards = Hazards.GetHazardsInRadius(query.Centre, query.Radius, query.Time);
                var heatmapsResponse = HazardsToHeatmaps.ConvertToHeatmapResponse(
                    query.Area,
                    query.NumberLonPoints,
                    intersectionHazards
                    );
                return new ObjectResult(heatmapsResponse);
            }
            else
            {
                var response = FutureTimeToHeatmap.ConvertToHeatmapResponse(query, HazardsToHeatmaps, Hazards);
                return new ObjectResult(response);
            }
        }
    }
}
