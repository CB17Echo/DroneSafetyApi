using DroneSafetyApi.Models;
using DroneSafetyApi.Services;
using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using Xunit.Extensions;

namespace DroneSafetyApi.UnitTests.ServicesTests
{
    public class HeatmapTests
    {
        [Theory, MemberData(nameof(ServiceValidationExamples_HeatmapExamples))]
        public void ListisEmpty_WhenNewHeatmapIsCreated(Bounds area, int numberLonPoints)
        {
            // Arrange
            var heatmap = new HeatMap(area, numberLonPoints);

            // Act
            var heatmapPointList = heatmap.GetHeatMapPoints();

            // Assert
            Assert.Empty(heatmapPointList);
        }

        public static object[] ServiceValidationExamples_HeatmapExamples
        {
            get
            {
                Bounds area1 = new Bounds(new Position(0, 0), new Position(1, 1));
                Bounds area2 = new Bounds(new Position(-3.4, 0.0012), new Position(4.23, 0.0021));
                return new[]
                {
                    new object[] { area1, 10, true },
                    new object[] { area2, 200, true },
                };
            }
        }

        [Theory, MemberData(nameof(CircleExamples))]
        public void ProcessCircle_CorrectlyInsertsCriclesIntoHeatMap(Bounds area, int numberLonPoints,
            Point circleCentre, int radius, Position[] positions, bool isValid)
        {
            // Arrange
            var heatmap = new HeatMap(area, numberLonPoints);

            // Act
            heatmap.ProcessCircle(circleCentre, radius, 1);
            var points = heatmap.Map.Keys;

            // Assert
            if (isValid)
            {
                foreach (Position pos in positions)
                {
                    Assert.Contains<Position>(pos, points);
                }
            } else
            {
                foreach (Position pos in positions)
                {
                    Assert.DoesNotContain(pos, points);
                }
            }
        }
        public static object[] CircleExamples
        {
            get
            {
                Bounds area1 = new Bounds(new Position(0, 0), new Position(10, 10));
                Bounds area2 = new Bounds(new Position(-3.4, 0.0012), new Position(4.23, 0.0021));
                return new[]
                {

                    new object[] { area1, 100, new Point(new Position(5,5)), 3 * HeatMap.MetresInLatDegree,
                        new Position[] { new Position(5,5), new Position(4.5,4.5), new Position(7,7) }, true },
                    new object[] { area1, 100, new Point(new Position(5,5)), 3 * HeatMap.MetresInLatDegree,
                        new Position[] { new Position(1,2), new Position(5,9), new Position(8.01,8.01) }, false },
                    new object[] { area1, 100, new Point(new Position(5,5)), 3 * HeatMap.MetresInLatDegree,
                        new Position[] { new Position(5.01, 5.01), new Position(4.49,4.49) }, false }
                };
            }
        }
    }
}