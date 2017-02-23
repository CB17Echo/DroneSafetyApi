using DroneSafetyApi.Models;
using DroneSafetyApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using DroneSafetyApi.Data;
using System;

namespace DroneSafetyApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class HeatmapsController : Controller
    {
        const int MetresInLatDegree = 110575;

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
            var radius = (int) (Math.Sqrt(((query.Area.Max.Latitude - query.Area.Min.Latitude) *
                (query.Area.Max.Latitude - query.Area.Min.Latitude)) +
                ((query.Area.Max.Longitude - query.Area.Min.Longitude) *
                (query.Area.Max.Longitude - query.Area.Min.Longitude)))) * MetresInLatDegree;
            var intersectionHazards = DataPoints.GetDataPointsInRadius(query.Centre, radius, new DateTime(2017,3,8,12,0,0));
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
