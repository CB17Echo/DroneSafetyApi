using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;
using System.Collections.Generic;

namespace DroneSafetyApi.Services
{
    public interface IDataPointsToHeatmapsResponse
    {
        HeatmapsResponse ConvertToHeatmapResponse(
            BoundingBox area,
            int width,
            int height,
            IEnumerable<DataPoint> datapoints);
    }
}
