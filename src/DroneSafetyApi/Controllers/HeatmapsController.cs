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
        /// The GetHeatmaps method generates a <see cref="HeatmapsResponse"/> from a <see cref="HeatmapsQuery"/> and serves it back as an HTTP response
        /// </summary>
        /// <param name="query"> The <see cref="HeatmapsQuery"/> used to create the <see cref="HeatmapsResponse"/> </param>
        /// <returns>A <see cref="HeatmapsResponse"/> containing the information received from the query</returns>
        [HttpGet]
        public IActionResult GetHeatmaps([FromQuery]HeatmapsQuery query)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }
            var intersectionHazards = Hazards.GetHazardsInRadius(query.Centre, query.Radius, query.Time);
            var heatmapsResponse = HazardsToHeatmaps.ConvertToHeatmapResponse(
                query.Area,
                query.NumberLonPoints,
                intersectionHazards
                );
            return new ObjectResult(heatmapsResponse);
        }
    }
}
