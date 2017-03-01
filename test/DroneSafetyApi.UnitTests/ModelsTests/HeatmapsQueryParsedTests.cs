using DroneSafetyApi.Models;
using DroneSafetyApi.Services;
using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DroneSafetyApi.UnitTests.ModelsTests
{
    public class HeatmapsQueryParsedTests
    {
        [Theory, MemberData(nameof(CornersExamples))]
        public void Area_ReturnsTheCorrectBoundingBox(double cornerOneLon, double cornerOneLat, double cornerTwoLon, double cornerTwoLat)
        {
            // Arrange
            var heatmapsQuery = new HeatmapsQuery
            {
                CornerOneLon = cornerOneLon,
                CornerOneLat = cornerOneLat,
                CornerTwoLon = cornerTwoLon,
                CornerTwoLat = cornerTwoLat
            };
            var heatmapsQueryParsed = new HeatmapsQueryParsed(heatmapsQuery);

            // Act
            var boundingBox = heatmapsQueryParsed.Area;

            // Assert
            Assert.Equal(Math.Max(cornerOneLon, cornerTwoLon), boundingBox.Max.Longitude);
            Assert.Equal(Math.Min(cornerOneLon, cornerTwoLon), boundingBox.Min.Longitude);
            Assert.Equal(Math.Max(cornerOneLat, cornerTwoLat), boundingBox.Max.Latitude);
            Assert.Equal(Math.Min(cornerOneLat, cornerTwoLat), boundingBox.Min.Latitude);
        }

        [Theory, MemberData(nameof(CornersExamples))]
        public void Centre_ReturnsTheCorrectCentrePoint(double cornerOneLon, double cornerOneLat, double cornerTwoLon, double cornerTwoLat)
        {
            // Arrange
            var heatmapsQuery = new HeatmapsQuery
            {
                CornerOneLon = cornerOneLon,
                CornerOneLat = cornerOneLat,
                CornerTwoLon = cornerTwoLon,
                CornerTwoLat = cornerTwoLat
            };
            var heatmapsQueryParsed = new HeatmapsQueryParsed(heatmapsQuery);
            var centralLongitude = Math.Min(cornerOneLon, cornerTwoLon) + Math.Abs(cornerOneLon - cornerTwoLon) / 2;
            var centralLatitude = Math.Min(cornerOneLat, cornerTwoLat) + Math.Abs(cornerOneLat - cornerTwoLat) / 2;

            // Act
            var centre = heatmapsQueryParsed.Centre;

            // Assert
            Assert.Equal(centralLongitude, centre.Position.Longitude);
            Assert.Equal(centralLatitude, centre.Position.Latitude);
        }

        [Theory, MemberData(nameof(CornersExamples))]
        public void Radius_ReturnsTheDistanceFromCentreToVertex(
            double cornerOneLon,
            double cornerOneLat,
            double cornerTwoLon,
            double cornerTwoLat)
        {
            // Arrange
            var heatmapsQuery = new HeatmapsQuery
            {
                CornerOneLon = cornerOneLon,
                CornerOneLat = cornerOneLat,
                CornerTwoLon = cornerTwoLon,
                CornerTwoLat = cornerTwoLat
            };
            var heatmapsQueryParsed = new HeatmapsQueryParsed(heatmapsQuery);
            var centre = heatmapsQueryParsed.Centre;
            var boundingBox = heatmapsQueryParsed.Area;
            var centreToVertex = PythagoreanDistanceInMetres(centre.Position, boundingBox.Max);

            // Act
            var radius = heatmapsQueryParsed.Radius;

            // Assert
            Assert.Equal(centreToVertex, radius);
        }

        private int PythagoreanDistanceInMetres(Position pointOne, Position pointTwo)
        {
            double deltaLongitude = pointOne.Longitude - pointTwo.Longitude;
            double deltaLatitude = pointOne.Latitude - pointTwo.Latitude;
            return ((int)Math.Sqrt(deltaLongitude * deltaLongitude + deltaLatitude * deltaLatitude)) * Heatmap.MetresInLatDegree;
        }

        public static object[] CornersExamples
        {
            get
            {
                return new[]
                {
                    new object[] { -5.0, -5.0, 5.0, 5.0 },
                    new object[] { 5.0, -5.0, -15.1223, 10.123 },
                    new object[] { -10.3738282, 67.223, -5.0, 8.33 },
                    new object[] { 5.0, -15.3, 1.3399, -24.33 }
                };
            }
        }
    }
}
