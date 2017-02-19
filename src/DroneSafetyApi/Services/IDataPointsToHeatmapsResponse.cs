using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;
using System.Collections.Generic;

namespace DroneSafetyApi.Services
{
    public interface IDataPointsToHeatmapsResponse
    {
        HeatmapsResponse ConvertToHeatmapResponse(
            double resolution,
            BoundingBox area,
            IEnumerable<DataPoint> datapoints);
    }
}
