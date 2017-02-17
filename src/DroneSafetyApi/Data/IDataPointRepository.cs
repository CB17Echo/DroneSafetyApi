using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Spatial;
using DroneSafetyApi.Models;


// Fix namespace in all the data classes and their Dependencies
namespace DroneSafetyApi.Data
{
    public interface IDataPointRepository
    {
        IEnumerable<DataPoint> GetDataPointsInRadius(Point point, int radius);
    }
}
