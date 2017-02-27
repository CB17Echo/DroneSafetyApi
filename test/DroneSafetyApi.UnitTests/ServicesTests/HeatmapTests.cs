using DroneSafetyApi.Models;
using DroneSafetyApi.Services;
using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using Moq;

namespace DroneSafetyApi.UnitTests.ServicesTests
{
    public class HeatmapTests
    {
        [Fact]
        public void ListisEmpty_WhenNewHeatmapIsCreated()
        {
            // Arrange
            var area = new Bounds(new Position(0, 0), new Position(1, 1));
            var heatmap = new Heatmap(area, It.IsAny<int>());

            // Act
            var heatmapPoints = heatmap.GetHeatmapPoints();

            // Assert
            Assert.Empty(heatmapPoints);
        }

        [Theory, MemberData(nameof(PointExamples))]
        public void ProcessPoint_CorrectlyInsertsPointsIntoHeatMap(Bounds area, int numberLonPoints,
            Point point, int severity, Position position, bool shouldHavePoints)
        {
            // Arrange
            var heatmap = new Heatmap(area, numberLonPoints);

            // Act
            heatmap.ProcessPoint(point, severity);
            var heatmapPoints = heatmap.GetHeatmapPoints();

            // Assert
            if (shouldHavePoints)
            {
                Assert.Contains(heatmapPoints, heatmapPoint =>
                    (heatmapPoint.X == position.Longitude)
                    && (heatmapPoint.Y == position.Latitude)
                    && (heatmapPoint.Value == severity));
                Assert.Equal(1, IEnumCount(heatmapPoints));
            }
            else
            {
                Assert.Empty(heatmapPoints);
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

                    new object[] { area1, 100, new Point(5,5), 1, new Position(5,5), true },
                    new object[] { area1, 100, new Point(5,5), 10, new Position(6,5), false },
                    new object[] { area1, 100, new Point(-1,11), 1, new Position(-1,11), false },
                    new object[] { area1, 100, new Point(2.11,7.87), 42, new Position(2.1,7.9), true },
                    new object[] { area1, 100, new Point(2.11,7.87), 1, new Position(2.11,7.87), false },
                    new object[] { area2, 500, new Point(0.09, 52.21), 1000, new Position(0.090046691894531253, 52.209983825683594), true },
                    new object[] { area2, 500, new Point(0.09, 52.21), 12, new Position(0.09, 52.21), false },
                };
            }
        }
        
        [Theory, MemberData(nameof(CircleExamples))]
        public void ProcessCircle_CorrectlyInsertsCirclesIntoHeatMap(Bounds area, int numberLonPoints,
            Point circleCentre, int radius, int severity, Position[] positions, bool isValid)
        {
            // Arrange
            var heatmap = new Heatmap(area, numberLonPoints);
            
            // Act
            heatmap.ProcessCircle(circleCentre, radius, severity);
            var points = heatmap.GetHeatmapPoints();
            var pointCount = IEnumCount(points);
            var resolution = (area.Max.Longitude - area.Min.Longitude) / numberLonPoints;
            var numberLatPoints = (int)((area.Max.Latitude - area.Min.Latitude) / resolution);
            var idealPointNum = (int)((Math.PI * radius * radius) * (numberLonPoints * numberLatPoints));
            var errorRange = (int)(idealPointNum * 0.001);
            
            // Assert
            Assert.InRange(pointCount, idealPointNum - errorRange, idealPointNum + errorRange);
            if (isValid)
            {
                foreach (Position position in positions)
                {
                    var heatmapPoint = points.FirstOrDefault(x => x.X == position.Longitude &&
                        x.Y == position.Latitude && x.Value == severity);
                    Assert.NotNull(heatmapPoint);
                }
            } else
            {
                foreach (Position position in positions)
                {
                    var heatmapPoint = points.FirstOrDefault(x => x.X == position.Longitude &&
                        x.Y == position.Latitude && x.Value == severity);
                    Assert.Null(heatmapPoint);
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

                    new object[] { area1, 100, new Point(5,5), 3 * Heatmap.MetresInLatDegree, 1,
                        new Position[] { new Position(5,5), new Position(4.5,4.5), new Position(7,7) }, true },
                    new object[] { area1, 100, new Point(5,5), 3 * Heatmap.MetresInLatDegree, 1,
                        new Position[] { new Position(1,2), new Position(5,9), new Position(8.01,8.01) }, false },
                    new object[] { area1, 100, new Point(5,5), 3 * Heatmap.MetresInLatDegree, 1,
                        new Position[] { new Position(5.01, 5.01), new Position(4.49,4.49) }, false },
                    new object[] { area2, 500, new Point(0.12, 52.203), 600, 1, goodPositions, true },
                    new object[] { area2, 500, new Point(0.12, 52.203), 600, 1, badPositions, false },
                };
            }
        }
        
        [Theory, MemberData(nameof(PolygonExamples))]
        public void ProcessPolygon_CorrectlyInsertsPolygonsIntoHeatMap(Bounds area, int numberLonPoints,
            Polygon polygon, int severity, Position[] positions, bool isValid)
        {
            // Arrange
            var heatmap = new Heatmap(area, numberLonPoints);

            // Act
            heatmap.ProcessPolygon(polygon, severity);
            var points = heatmap.GetHeatmapPoints();
            var pointCount = IEnumCount(points);
            var maxArea = MaxPolygonArea(polygon);
            var boundsArea = (area.Max.Longitude - area.Min.Longitude) * (area.Max.Latitude - area.Min.Latitude);
            var resolution = (area.Max.Longitude - area.Min.Longitude) / numberLonPoints;
            var numberLatPoints = (int)((area.Max.Latitude - area.Min.Latitude) / resolution);
            var maxPointNum = maxArea / boundsArea * numberLonPoints * numberLonPoints;

            // Assert
            Assert.InRange(pointCount, 0, maxPointNum);
            if (isValid)
            {
                foreach (Position position in positions)
                {
                    var heatmapPoint = points.FirstOrDefault(x => x.X == position.Longitude &&
                        x.Y == position.Latitude && x.Value == severity);
                    Assert.NotNull(heatmapPoint);
                }
            } else
            {
                foreach (Position position in positions)
                {
                    var heatmapPoint = points.FirstOrDefault(x => x.X == position.Longitude &&
                        x.Y == position.Latitude && x.Value == severity);
                    Assert.Null(heatmapPoint);
                }
            }
        }
        public static object[] PolygonExamples
        {
            get
            {
                Bounds area1 = new Bounds(new Position(0, 0), new Position(10, 10));
                Bounds area2 = new Bounds(new Position(0.0707244873046875, 52.170983918129096), new Position(0.1847076416015625, 52.230743427331866));
                Polygon poly = new Polygon(new[]
                {
                    new Position(2,2),
                    new Position(4, 2),
                    new Position(4,4),
                    new Position(3,5),
                    new Position(2,4),
                    new Position(2,2)
                });
                Polygon poly1 = new Polygon(new[]
                {
                    new Position(0.11, 52.203),
                    new Position(0.12, 52.203),
                    new Position(0.12, 52.213),
                    new Position(0.11, 52.213),
                    new Position(0.11, 52.203)
                });
                Polygon poly2 = new Polygon(new[]
                {
                    new Position(0.081, 52.193),
                    new Position(0.14, 52.207),
                    new Position(0.147, 52.225),
                    new Position(0.158, 52.213),
                    new Position(0.111, 52.21),
                    new Position(0.071, 52.182),
                    new Position(0.081, 52.193)
                });
                Position[] goodPoly1Positions = new Position[]
                {
                    new Position(0.110335693359375, 52.206792297363279),
                    new Position(0.11193145751953125, 52.207932128906251),
                    new Position(0.11626281738281249, 52.21226348876953),
                    new Position(0.11968231201171875, 52.203372802734371),
                    new Position(0.113983154296875, 52.212947387695309)
                };
                Position[] badPoly1Positions = new Position[]
                {
                    new Position(0.11626281738281249,52.199269409179685),
                    new Position(0.0,51),
                    new Position(0.11, 52.203),
                    new Position(0.11854248046875,52.199497375488278)
                };
                Position[] goodPoly2Positions1 = new Position[]
                {
                    new Position(0.0715814208984375, 52.182627868652339),
                    new Position(0.077508544921874992, 52.187643127441405),
                    new Position(0.10212890625, 52.199269409179685),
                    new Position(0.157752685546875, 52.2131753540039)
                };
                Position[] badPoly2Positions1 = new Position[]
                {
                    new Position(0.147, 52.225),
                    new Position(0.158, 52.213),
                    new Position(-1,-1),
                    new Position(0.0916424560546875, 52.21043975830078)
                };
                Position[] goodPoly2Positions2 = new Position[]
                {
                    new Position(0.071809387207031256, 52.182627868652347),
                    new Position(0.13791961669921876, 52.211123657226565),
                    new Position(0.15159759521484376, 52.215682983398438)
                };
                Position[] badPoly2Positions2 = new Position[]
                {
                    new Position(0.0715814208984375, 52.182627868652339),
                    new Position(0.158, 52.213),
                    new Position(-1,-1)
                };
                return new[]
                {
                    new object[] {area1, 100, poly, 1, new Position[] { }, true },
                    new object[] { area2, 500, poly1, 1, goodPoly1Positions, true },
                    new object[] { area2, 500, poly1, 1, badPoly1Positions, false },
                    new object[] { area2, 500, poly2, 1, goodPoly2Positions1, true },
                    new object[] { area2, 500, poly2, 1, badPoly2Positions1, false },
                    new object[] { area2, 100, poly2, 1, goodPoly2Positions2, true },
                    new object[] { area2, 100, poly2, 1, badPoly2Positions2, false },
                };
            }
        }

        private int IEnumCount<T>(IEnumerable<T> collection)
        {
            int count = 0;
            foreach(T item in collection)
            {
                count++;
            }
            return count;
        }

        private double MaxPolygonArea(Polygon polygon)
        {
            IList<Position> coord = polygon.Rings[0].Positions;
            double minLong = coord[0].Longitude;
            double maxLong = coord[0].Longitude;
            double minLat = coord[0].Latitude;
            double maxLat = coord[0].Latitude;
            for (int i = 1; i < coord.Count - 1; i++)
            {
                if (coord[i].Latitude < minLat) { minLat = coord[i].Latitude; }
                else if (coord[i].Latitude > maxLat) { maxLat = coord[i].Latitude; }
                if (coord[i].Longitude < minLong) { minLong = coord[i].Longitude; }
                else if (coord[i].Longitude > maxLong) { maxLong = coord[i].Longitude; }
            }
            return (maxLong - minLong) * (maxLat - minLat);
        }
    }
}