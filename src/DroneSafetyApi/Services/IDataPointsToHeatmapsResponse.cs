using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;
using System.Collections.Generic;

namespace DroneSafetyApi.Services
{
    public interface IDataPointsToHeatmapsResponse
    {
        HeatmapsResponse ConvertToHeatmapResponse(
            BoundingBox area,
            IEnumerable<DataPoint> datapoints);
    }
}
