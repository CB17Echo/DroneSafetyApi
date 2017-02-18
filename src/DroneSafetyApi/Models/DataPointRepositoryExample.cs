using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Models
{
    public class DataPointRepositoryExample : IDataPointRepository
    {
        public IEnumerable<DataPoint> GetDataPointsInRadius(double x, double y, int radius)
        {
            List<DataPoint> datapoints = new List<DataPoint>();
            DataPoint a1 = new DataPoint();
            a1.DataType = "Source1";
            a1.Shape = "Polygon";
            a1.Severity = 2;
            a1.Location = new Polygon(new []
            {
                new Point()
            })
        public int Data_ID { get; set; }
    }
    }
}
