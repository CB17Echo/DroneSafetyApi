using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;
using System.Collections.Generic;

namespace DroneSafetyApi.Data
{
    public class ExamplesDataRespository : IHazardRepository
    {
        public IEnumerable<Hazard> GetHazardsInRadius(Point point, int radius)
        {

            return new Hazard[] 
            {
                new Hazard
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
                new Hazard
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
                new Hazard
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
