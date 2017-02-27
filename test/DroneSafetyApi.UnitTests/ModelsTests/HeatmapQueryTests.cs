using DroneSafetyApi.Models;
using DroneSafetyApi.Services;
using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using Xunit.Extensions;

namespace DroneSafetyApi.UnitTests.ModelsTests
{
    public class HeatmapsQueryTests
    {
        [Theory, MemberData(nameof(ModelValidationExamples_NumberLonPointsRange))]
        public void ModelIsInvalid_WhenHeightOrWidthIsNonPositive(int numberLonPoints, bool isValid)
        {
            // Arrange
            var heatmapsQuery = new HeatmapsQuery { NumberLonPoints = numberLonPoints };
            var validationContext = new ValidationContext(heatmapsQuery, null, null);
            var results = new List<ValidationResult>();

            // Act
            Validator.TryValidateObject(heatmapsQuery, validationContext, results, true);

            // Assert
            if (isValid)
            {
                Assert.Empty(results);
            }
            else
            {
                Assert.NotEmpty(results);
            }
        }

        public static object[] ModelValidationExamples_NumberLonPointsRange
        {
            get
            {
                return new[]
                {
                    new object[] { 10, true },
                    new object[] { 20000, true },
                    new object[] { 0, false },
                    new object[] { -5, false },
                    new object[] { -300, false },
                    new object[] { 1, true }
                };
            }
        }

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

            // Act
            var boundingBox = heatmapsQuery.Area;

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
            var centralLongitude = Math.Min(cornerOneLon, cornerTwoLon) + Math.Abs(cornerOneLon - cornerTwoLon) / 2;
            var centralLatitude = Math.Min(cornerOneLat, cornerTwoLat) + Math.Abs(cornerOneLat - cornerTwoLat) / 2;

            // Act
            var centre = heatmapsQuery.Centre;

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
            var centre = heatmapsQuery.Centre;
            var boundingBox = heatmapsQuery.Area;
            var centreToVertex = PythagoreanDistanceInMetres(centre.Position, boundingBox.Max);

            // Act
            var radius = heatmapsQuery.Radius;

            // Assert
            Assert.Equal(centreToVertex, radius);
        }

        private int PythagoreanDistanceInMetres(Position pointOne, Position pointTwo)
        {
            double deltaLongitude = pointOne.Longitude - pointTwo.Longitude;
            double deltaLatitude = pointOne.Latitude - pointTwo.Latitude;
            return ((int) Math.Sqrt(deltaLongitude * deltaLongitude + deltaLatitude * deltaLatitude)) * Heatmap.MetresInLatDegree;
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