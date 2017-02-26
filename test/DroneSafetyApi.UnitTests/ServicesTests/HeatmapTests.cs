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
                    new object[] { area1, 10 },
                    new object[] { area2, 200 },
                };
            }
        }

        [Theory, MemberData(nameof(PointExamples))]
        public void ProcessPoint_CorrectlyInsertsPointsIntoHeatMap(Bounds area, int numberLonPoints,
            Point point, Position position, bool isValid)
        {
            // Arrange
            var heatmap = new HeatMap(area, numberLonPoints);
            var x = position.Longitude;
            var y = position.Latitude;
            var heatmapPoint = new HeatmapPoint { X = position.Longitude,
                Y = position.Latitude, Value = 1 };

            // Act
            heatmap.ProcessPoint(point, 1);
            var points = heatmap.GetHeatMapPoints();

            // Assert
            if (isValid)
            {
                Assert.Contains(heatmapPoint, points);
            }
            else
            {
                Assert.DoesNotContain(heatmapPoint, points);
            }
        }
        public static object[] PointExamples
        {
            get
            {
                Bounds area1 = new Bounds(new Position(0, 0), new Position(10, 10));
                Bounds area2 = new Bounds(new Position(0.0707244873046875, 52.170983918129096), new Position(0.1847076416015625, 52.230743427331866));
                return new[]
                {

                    new object[] { area1, 100, new Point(5,5), new Position(5,5), true },
                    new object[] { area1, 100, new Point(5,5), new Position(6,5), false },
                    new object[] { area1, 100, new Point(2.11,7.87), new Position(2.1,7.9), true },
                    new object[] { area1, 100, new Point(2.11,7.87), new Position(2.11,7.87), false },
                    new object[] { area2, 500, new Point(0.09, 52.21), new Position(0.090046691894531253, 52.209983825683594), true },
                    new object[] { area2, 500, new Point(0.09, 52.21), new Position(0.09, 52.21), false },
                };
            }
        }
        /*
        [Theory, MemberData(nameof(CircleExamples))]
        public void ProcessCircle_CorrectlyInsertsCirclesIntoHeatMap(Bounds area, int numberLonPoints,
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
                    Assert.Contains(pos, points);
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
                Bounds area2 = new Bounds(new Position(0.0707244873046875, 52.170983918129096), new Position(0.1847076416015625, 52.230743427331866));
                Position[] goodPositions = new Position[]
                {
                    new Position(0.1171746826171875,52.207020263671872),
                    new Position(0.11785858154296874,52.205196533203122),
                    new Position(0.11899841308593749,52.198357543945313),
                    new Position(0.12105010986328124,52.199269409179685),
                    new Position(0.12515350341796874,52.202232971191407)
                };
                Position[] badPositions = new Position[]
                {
                    new Position(0.11512298583984375,52.21226348876953),
                    new Position(0.110335693359375,52.212035522460937),
                    new Position(0.11170349121093749,52.211807556152344)
                };
                return new[]
                {

                    new object[] { area1, 100, new Point(5,5), 3 * HeatMap.MetresInLatDegree,
                        new Position[] { new Position(5,5), new Position(4.5,4.5), new Position(7,7) }, true },
                    new object[] { area1, 100, new Point(5,5), 3 * HeatMap.MetresInLatDegree,
                        new Position[] { new Position(1,2), new Position(5,9), new Position(8.01,8.01) }, false },
                    new object[] { area1, 100, new Point(5,5), 3 * HeatMap.MetresInLatDegree,
                        new Position[] { new Position(5.01, 5.01), new Position(4.49,4.49) }, false },
                    new object[] { area2, 500, new Point(0.12, 52.203), 600, goodPositions, true },
                    new object[] { area2, 500, new Point(0.12, 52.203), 600, badPositions, false },
                };
            }
        }

        [Theory, MemberData(nameof(PolygonExamples))]
        public void ProcessPolygon_CorrectlyInsertsPolygonsIntoHeatMap(Bounds area, int numberLonPoints,
            Polygon polygon, Position[] positions, bool isValid)
        {
            // Arrange
            var heatmap = new HeatMap(area, numberLonPoints);

            // Act
            heatmap.ProcessPolygon(polygon, 1);
            var points = heatmap.Map.Keys;

            // Assert
            if (isValid)
            {
                foreach (Position pos in positions)
                {
                    Assert.Contains(pos, points);
                }
            }
            else
            {
                foreach (Position pos in positions)
                {
                    Assert.DoesNotContain(pos, points);
                }
            }
        }
        public static object[] PolygonExamples
        {
            get
            {
                Bounds area1 = new Bounds(new Position(0, 0), new Position(10, 10));
                Bounds area2 = new Bounds(new Position(0.0707244873046875, 52.170983918129096), new Position(0.1847076416015625, 52.230743427331866));
                Position[] goodPoly1Positions = new Position[]
                {
                    
                };
                Position[] badPoly1Positions = new Position[]
                {
                    
                };
                return new[]
                {

                    new object[] { area1, 100, new Point(5,5), 3 * HeatMap.MetresInLatDegree,
                        new Position[] { new Position(5,5), new Position(4.5,4.5), new Position(7,7) }, true },
                    new object[] { area1, 100, new Point(5,5), 3 * HeatMap.MetresInLatDegree,
                        new Position[] { new Position(1,2), new Position(5,9), new Position(8.01,8.01) }, false },
                    new object[] { area1, 100, new Point(5,5), 3 * HeatMap.MetresInLatDegree,
                        new Position[] { new Position(5.01, 5.01), new Position(4.49,4.49) }, false },
                    new object[] { area2, 500, new Point(0.12, 52.203), 600, goodPositions, true },
                    new object[] { area2, 500, new Point(0.12, 52.203), 600, badPositions, false },
                };
            }
        }*/
    }
}