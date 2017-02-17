using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Models
{
    public class ExamplesDataRespository : IDataPointRepository
    {
        public IEnumerable<DataPoint> GetDataPointsInRadius(Point point, int radius)
        {

            return new DataPoint[] 
            {
                new DataPoint
                {
                    DataType = "Bus",
                    Shape = "Polygon",
                    Severity = 5,
                    Location = new Polygon(
                        new[]
                        {
                            new Position(0.11, 52.203),
                            new Position(0.12, 52.203),
                            new Position(0.12, 52.205),
                            new Position(0.11, 52.205),
                            new Position(0.11, 52.203)
                        }),
                    Data_ID = 5
                },
                new DataPoint
                {
                    DataType = "Car",
                    Shape = "Polygon",
                    Severity = 5,
                    Location = new Polygon(
                        new[]
                        {
                            new Position(0.11, 52.203),
                            new Position(0.12, 52.203),
                            new Position(0.12, 52.205),
                            new Position(0.11, 52.205),
                            new Position(0.11, 52.203)
                        }),
                    Data_ID = 5
                },
                new DataPoint
                {
                    DataType = "I",
                    Shape = "Point",
                    Severity = 50,
                    Location = new Point(0.115, 52.204),
                    Data_ID = 8
                }
            };
        }
    }
}
