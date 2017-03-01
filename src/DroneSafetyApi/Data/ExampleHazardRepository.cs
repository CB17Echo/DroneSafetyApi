using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Data
{
    public class ExampleHazardRepository : IHazardRepository
    {
        private Hazard[] hazards = new Hazard[]
            {
                new PolygonalHazard
                {
                    DataType = "Bus",
                    Shape = "Polygon",
                    Severity = 40,
                    Location = new Polygon(
                        new[]
                        {
                            new Position(0.11, 52.203),
                            new Position(0.12, 52.203),
                            new Position(0.12, 52.213),
                            new Position(0.11, 52.213),
                            new Position(0.11, 52.203)
                        }),
                    StartTime = new DateTime(2017,2,1,23,45,0),
                    EndTime = new DateTime(2017,2,2,00,15,0)
                },
                new PolygonalHazard
                {
                    DataType = "Car",
                    Shape = "Polygon",
                    Severity = 30,
                    Location = new Polygon(
                        new[]
                        {
                            new Position(0.081, 52.193),
                            new Position(0.14, 52.207),
                            new Position(0.147, 52.225),
                            new Position(0.158, 52.213),
                            new Position(0.111, 52.21),
                            new Position(0.071, 52.182),
                            new Position(0.081, 52.193)
                        }),
                    StartTime = new DateTime(2017,2,1,23,45,0),
                    EndTime = new DateTime(2017,2,2,00,15,0)
                },
                new PointHazard
                {
                    DataType = "Wifi",
                    Shape = "Point",
                    Severity = 8,
                    Location = new Point(0.09, 52.21),
                    StartTime = new DateTime(2017,2,1,23,45,0),
                    EndTime = new DateTime(2017,2,2,00,15,0)
                }, new CircularHazard
                {
                    DataType = "Wifi",
                    Shape = "Circle",
                    Severity = 25,
                    Location = new Point(0.09, 52.21),
                    StartTime = new DateTime(2017,2,1,23,45,0),
                    EndTime = new DateTime(2017,2,2,00,15,0),
                    Radius = 212
                },
                new CircularHazard
                {
                    DataType = "Bus",
                    Shape = "Circle",
                    Severity = 15,
                    Location = new Point(0.12, 52.203),
                    StartTime = new DateTime(2017,2,1,23,45,0),
                    EndTime = new DateTime(2017,2,2,00,15,0),
                    Radius = 600
                },
                new PolygonalHazard
                {
                    DataType = "Bus",
                    Shape = "Polygon",
                    Severity = 50,
                    Location = new Polygon(
                        new[]
                        {
                            new Position(0.158, 52.213),
                            new Position(0.14, 52.207),
                            new Position(0.11, 52.22),
                            new Position(0.158, 52.213)
                        }),
                    StartTime = new DateTime(2017,3,1,10,0,0),
                    EndTime = new DateTime(2017,3,1,10,30,0)
                },
                new PointHazard
                {
                    DataType = "Wifi",
                    Shape = "Point",
                    Severity = 12,
                    Location = new Point(0.11, 52.21),
                    StartTime = new DateTime(2017,3,1,10,0,0),
                    EndTime = new DateTime(2017,3,1,10,30,0)
                }, new CircularHazard
                {
                    DataType = "Wifi",
                    Shape = "Circle",
                    Severity = 70,
                    Location = new Point(0.12, 52.213),
                    StartTime = new DateTime(2017,3,1,10,0,0),
                    EndTime = new DateTime(2017,3,1,10,30,0),
                    Radius = 300
                },
            };


        public IEnumerable<Hazard> GetHazardsInRadius(Point point, int radius, DateTime time)
        {
            List<Hazard> hazardsInRadius = new List<Hazard>();
            foreach(Hazard hazard in hazards)
            {
                if(hazard.StartTime <= time && hazard.EndTime >= time)
                {
                    hazardsInRadius.Add(hazard);
                }
            }
            return hazardsInRadius;
        }
    }
}
