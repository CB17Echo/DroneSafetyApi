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
    /// <summary>
    /// The HeatmapsController class is a controller to recieve queries from and return <see cref="HeatmapsResponse"/>s  to
    /// the web application
    /// </summary>
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class HeatmapsController : Controller
    {
        /// <summary>
        /// The Hazards property is used to query the database for a collection of <see cref="Hazard"/>s
        /// </summary>
        public IHazardRepository Hazards { get; set; }
        /// <summary>
        /// The HazardsToHeatmaps property is used to convert the queried hazards from <see cref="HeatmapsController.Hazards"/>
        /// into <see cref="IHazardsToHeatmapsResponse"/>s
        /// </summary>
        public IHazardsToHeatmapsResponse HazardsToHeatmaps { get; set; }
        /// <summary>
        /// Creates a new instance of the <see cref="HeatmapsController"/> class
        /// </summary>
        /// <param name="hazards"> The <see cref="IHazardRepository"/> used to query for <see cref="Hazards"/> in the new
        /// <see cref="HeatmapsController"/> instance </param>
        /// <param name="hazardsToHeatmaps"> The <see cref="IHazardsToHeatmapsResponse"/> used to generate <see cref="HeatmapsResponse"/>s
        /// in the new <see cref="HeatmapsController"/> instance</param>
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
