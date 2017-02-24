﻿using System;
using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;
using System.Collections.Generic;

namespace DroneSafetyApi.Data
{
    public class ExamplesDataRespository : IDataPointRepository
    {
        public IEnumerable<DataPoint> GetDataPointsInRadius(Point point, int radius, DateTime time)
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
                    Data_ID = 5,
                    Time = time
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
                    Data_ID = 5,
                    Time = time
                },
                new DataPoint
                {
                    DataType = "I",
                    Shape = "Point",
                    Severity = 50,
                    Location = new Point(0.115, 52.204),
                    Data_ID = 8,
                    Time = time
                }
            };
        }
    }
}