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
    public class HazardToHeatmapResponseTests
    {
        [Fact]
        public void ConvertToHeatmap_ReturnsValidHeatmap()
        {
            var mockHeatmap = new Mock<IHeatmap>();
            var mockHeatmapFactory = new Mock<IHeatmapFactory>();
            mockHeatmapFactory
                .Setup(factory => factory.CreateHeatmap(It.IsAny<Bounds>(), It.IsAny<int>()))
                .Returns(mockHeatmap.Object);
            var mockBounds = new Mock<Bounds>(new Position(It.IsAny<double>(), It.IsAny<double>()),
                new Position(It.IsAny<double>(), It.IsAny<double>()));
            var testHazards = new List<Hazard>();
            var hazardsToHeatmapResponse = new HazardsToHeatmapResponse(mockHeatmapFactory.Object);
            var heatmapsQuery = new HeatmapsQuery();

            // Act
            var result = hazardsToHeatmapResponse.ConvertToHeatmap(mockBounds.Object,
                It.IsAny<int>(), testHazards);

            // Assert
            Assert.IsType<IHeatmap>(result);
        }
    }
}