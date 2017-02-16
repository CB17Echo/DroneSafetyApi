using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Models
{
    public class DataPointRepository : IDataPointRepository
    {
        public IEnumerable<DataPoint> GetDataPointsInRadius(double x, double y, int radius)
        {
            DataPointDB.CreateDocumentClient();
            return DataPointDB.GetDataPoints(new Point(x, y), radius);
        }
    }
}
