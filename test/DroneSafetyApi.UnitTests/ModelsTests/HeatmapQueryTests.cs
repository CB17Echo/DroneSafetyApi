using DroneSafetyApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using Xunit.Extensions;

namespace DroneSafetyApi.UnitTests.ModelsTests
{
    public class HeatmapsQueryTests
    {
        [Theory, MemberData(nameof(ModelValidationExamples_HeigthWidthRange))]
        public void ModelIsInvalid_WhenHeightOrWidthIsNonPositive(int height, int width, bool isValid)
        {
            // Arrange
            var heatmapsQuery = new HeatmapsQuery { Height = height, Width = width };
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

        public static object[] ModelValidationExamples_HeigthWidthRange
        {
            get
            {
                return new[]
                {
                    new object[] { 10, 4, true },
                    new object[] { 200, 1, true },
                    new object[] { 0, 10000, false },
                    new object[] { -5, 20000, false },
                    new object[] { 5, 0, false },
                    new object[] { 2000, -5, false },
                    new object[] { 0, 0, false },
                    new object[] { -5, -20, false }
                };
            }
        }

        [Theory, MemberData(nameof(CornersExamples))]
        public void Area_ReturnsTheCorrectBoundingBox(double cornerOneLat, double cornerOneLon, double cornerTwoLat, double cornerTwoLon)
        {
            // Arrange
            var heatmapsQuery = new HeatmapsQuery
            {
                CornerOneLat = cornerOneLat,
                CornerOneLon = cornerOneLon,
                CornerTwoLat = cornerTwoLat,
                CornerTwoLon = cornerTwoLon
            };

            // Act
            var boundingBox = heatmapsQuery.Area;

            // Assert
            Assert.Equal(Math.Max(cornerOneLat, cornerTwoLat), boundingBox.Max.Latitude);
            Assert.Equal(Math.Min(cornerOneLat, cornerTwoLat), boundingBox.Min.Latitude);
            Assert.Equal(Math.Max(cornerOneLon, cornerTwoLon), boundingBox.Max.Longitude);
            Assert.Equal(Math.Min(cornerOneLon, cornerTwoLon), boundingBox.Min.Longitude);
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