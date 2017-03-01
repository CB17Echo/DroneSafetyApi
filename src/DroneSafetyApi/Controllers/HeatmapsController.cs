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

        /// <summary>
        /// Serves back a set of heatmaps, corresponding to the given query parameters
        /// </summary>
        /// <returns>A Heatmaps Response (see example) with the relevant heatmaps populated. </returns>
        /// <response code="200">Returns the required heatmaps.</response>
        /// <response code="400">If the query string is invalid.</response>
        [HttpGet]
        [ProducesResponseType(typeof(HeatmapsResponse), 200)]
        [ProducesResponseType(typeof(HeatmapsResponse), 400)]
        public IActionResult GetHeatmaps([FromQuery]HeatmapsQuery query)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }
            var heatmapsQueryParsed = new HeatmapsQueryParsed(query);
            var intersectionHazards = Hazards.GetHazardsInRadius(heatmapsQueryParsed.Centre, heatmapsQueryParsed.Radius, heatmapsQueryParsed.Time);
            var heatmapsResponse = HazardsToHeatmaps.ConvertToHeatmapResponse(
                heatmapsQueryParsed.Area,
                query.NumberLonPoints,
                intersectionHazards
                );
            return new ObjectResult(heatmapsResponse);
        }
    }
}
