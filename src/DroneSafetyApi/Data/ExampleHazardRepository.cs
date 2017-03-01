﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Data
{
    /// <summary>
    /// The ExampleHazardRepository class is a sample data model to provide fake hazards to the API rather than query a database
    /// </summary>
    public class ExampleHazardRepository : IHazardRepository
    {
        /// <summary>
        /// <see cref="IHazardRepository.GetHazardsInRadius(Point, int, DateTime)"/>
        /// </summary>
        public IEnumerable<Hazard> GetHazardsInRadius(Point location, int radius, DateTime time)
        {
            return new Hazard[]
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
                    StartTime = time,
                    EndTime = time
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
                    StartTime = time,
                    EndTime = time
                },
                new PointHazard
                {
                    DataType = "Wifi",
                    Shape = "Point",
                    Severity = 8,
                    Location = new Point(0.09, 52.21),
                    StartTime = time,
                    EndTime = time
                }, new CircularHazard
                {
                    DataType = "Wifi",
                    Shape = "Circle",
                    Severity = 25,
                    Location = new Point(0.09, 52.21),
                    StartTime = time,
                    EndTime = time,
                    Radius = 212
                },
                new CircularHazard
                {
                    DataType = "Bus",
                    Shape = "Circle",
                    Severity = 15,
                    Location = new Point(0.12, 52.203),
                    StartTime = time,
                    EndTime = time,
                    Radius = 600
                }
            };
        }
    }
}
