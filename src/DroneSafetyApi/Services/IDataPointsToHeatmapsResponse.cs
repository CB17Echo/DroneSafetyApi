using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Services
{
    public interface IDataPointsToHeatmapsResponse
    {
        HeatmapsResponse ConvertToHeatmapResponse(
            int width,
            int height,
            BoundingBox area,
            IEnumerable<DataPoint> datapoints);
    }
}
