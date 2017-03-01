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
        [Theory, MemberData(nameof(ModelValidationExamples))]
        public void ModelIsInvalid_WhenNumberLonPointsIsNonPositive(int numberLonPoints)
        {
            // Arrange
            var heatmapsQuery = new HeatmapsQuery { NumberLonPoints = numberLonPoints };
            var validationContext = new ValidationContext(heatmapsQuery, null, null);
            var results = new List<ValidationResult>();

            // Act
            Validator.TryValidateObject(heatmapsQuery, validationContext, results, true);

            // Assert
            if (numberLonPoints > 0)
            {
                Assert.Empty(results);
            }
            else
            {
                Assert.NotEmpty(results);
            }
        }

        [Theory, MemberData(nameof(ModelValidationExamples))]
        public void ModelIsInvalid_WhenUnixTimeIsNegative(long unixTime)
        {
            // Arrange
            var heatmapsQuery = new HeatmapsQuery { UnixTime = unixTime, NumberLonPoints = 1 };
            var validationContext = new ValidationContext(heatmapsQuery, null, null);
            var results = new List<ValidationResult>();

            // Act
            Validator.TryValidateObject(heatmapsQuery, validationContext, results, true);

            // Assert
            if (unixTime >= 0)
            {
                Assert.Empty(results);
            }
            else
            {
                Assert.NotEmpty(results);
            }
        }

        public static object[] ModelValidationExamples
        {
            get
            {
                return new[]
                {
                    new object[] { 10 },
                    new object[] { 20000 },
                    new object[] { 0 },
                    new object[] { -5 },
                    new object[] { -300 },
                    new object[] { 1 }
                };
            }
        }
    }
}