using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Models
{
    public class ExamplesDataRespository : IDataPointRepository
    {
        public IEnumerable<DataPoint> GetDataPointsInRadius(double x, double y, int radius)
        {
            return new DataPoint[] {
            new DataPoint {
                DataType = "Bus",
                Shape = "Polygon",
                Severity = 5,
                Location = //new Point(0.1847076416015, 52.230743427331),
                new Polygon(
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
            new DataPoint {
                DataType = "Car",
                Shape = "Polygon",
                Severity = 5,
                Location = //new Point(0.1847076416015, 52.230743427331),
                new Polygon(
                    new[]
                    {
                        new Position(0.11, 52.203),
                        new Position(0.12, 52.203),
                        new Position(0.12, 52.205),
                        new Position(0.11, 52.205),
                        new Position(0.11, 52.203)
                    }),
                Data_ID = 5
            }
            };
        }
    }
}
