using DroneSafetyApi.Controllers;
using DroneSafetyApi.Data;
using DroneSafetyApi.Models;
using DroneSafetyApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Spatial;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DroneSafetyApi.UnitTests.ControllersTests
{
    public class HeatmapsControllerTests
    {
        [Fact]
        public void GetHeatmaps_ReturnsBadRequestResult_WhenModelStateIsNotValid()
        {
            // Arrange
            var mockHazardRespository = new Mock<IHazardRepository>();
            var mockHazardsToHeatmapsResponse = new Mock<IHazardsToHeatmapsResponse>();
            var controller = new HeatmapsController(mockHazardRespository.Object, mockHazardsToHeatmapsResponse.Object);
            controller.ModelState.AddModelError("Width", "Witdh must be a positive Integer");
            var heatmapsQuery = new HeatmapsQuery();

            // Act
            var result = controller.GetHeatmaps(heatmapsQuery);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void GetHeatmaps_FetchesHazardsFromDatabase()
        {
            // Arrange
            var mockHazardRepository = new Mock<IHazardRepository>();
            mockHazardRepository
                .Setup(repository => repository.GetHazardsInRadius(It.IsAny<Point>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(new List<Hazard>())
                .Verifiable();
            var mockHazardsToHeatmapsResponse = new Mock<IHazardsToHeatmapsResponse>();
            var controller = new HeatmapsController(mockHazardRepository.Object, mockHazardsToHeatmapsResponse.Object);
            var heatmapsQuery = new HeatmapsQuery();

            // Act
            controller.GetHeatmaps(heatmapsQuery);

            // Assert
            mockHazardRepository.Verify();
        }

        [Fact]
        public void GetHeatmaps_ReturnsAnActionResult_WithCorrectHeatmapsResponse()
        {
            // Arrange
            var exampleHeatmapsResponse = CreateHeatmapsResponse();
            var mockHazardRepository = new Mock<IHazardRepository>();
            var mockHazardsToHeatmapsResponse = new Mock<IHazardsToHeatmapsResponse>();
            mockHazardsToHeatmapsResponse
                 .Setup(heatmapsResponseStrategy => heatmapsResponseStrategy.ConvertToHeatmapResponse(
                     It.IsAny<Bounds>(),
                     It.IsAny<int>(),
                     It.IsAny<int>(),
                     It.IsAny<IEnumerable<Hazard>>()))
                 .Returns(exampleHeatmapsResponse)
                 .Verifiable();
            var controller = new HeatmapsController(mockHazardRepository.Object, mockHazardsToHeatmapsResponse.Object);
            var heatmapsQuery = new HeatmapsQuery();

            // Act
            var result = controller.GetHeatmaps(heatmapsQuery);

            // Assert
            mockHazardsToHeatmapsResponse.Verify();
            var objectResult = Assert.IsType<ObjectResult>(result);
            var heatmapsResponse = Assert.IsAssignableFrom<HeatmapsResponse>(objectResult.Value);
            Assert.Equal(exampleHeatmapsResponse.NumSources, heatmapsResponse.NumSources);
            Assert.True(Enumerable.SequenceEqual(exampleHeatmapsResponse.Sources, heatmapsResponse.Sources));
            Assert.Empty(heatmapsResponse.Heatmaps);
        }

        private HeatmapsResponse CreateHeatmapsResponse()
        {
            return new HeatmapsResponse
            {
                NumSources = 3,
                Sources = new string[]
                {
                    "Bus",
                    "Wifi",
                    "Twitter"
                },
                Heatmaps = new Dictionary<string, IEnumerable<HeatmapPoint>>()
            };
        }
    }
}
