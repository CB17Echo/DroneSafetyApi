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
    public class HazardsToHeatmapResponseTests
    {
        [Fact]
        public void ConvertToHeatmap_CallsProcessPoint_WhenPointIsPresentInHazards()
        {
            // Arrange
            var mockHeatmap = new Mock<IHeatmap>(MockBehavior.Strict);
            // Strict makes sure only the setup methods are called
            mockHeatmap
                .Setup(mock => mock.ProcessPoint(It.IsAny<Point>(), It.IsAny<int>()))
                .Verifiable();
            var mockHeatmapFactory = new Mock<IHeatmapFactory>();
            mockHeatmapFactory
                .Setup(mock => mock.CreateHeatmap(It.IsAny<Bounds>(), It.IsAny<int>()))
                .Returns(mockHeatmap.Object)
                .Verifiable();
            var hazardsToHeatmapResponse = new HazardsToHeatmapResponse(mockHeatmapFactory.Object);
            var hazards = new Hazard[]
            {
                new PointHazard { Shape = "Point" }
            };

            // Act
            hazardsToHeatmapResponse.ConvertToHeatmap(It.IsAny<Bounds>(), It.IsAny<int>(), hazards);

            // Assert
            mockHeatmapFactory.Verify();
            mockHeatmap.Verify();
        }

        [Fact]
        public void ConvertToHeatmap_CallsProcessCircle_WhenCircleIsPresentInHazards()
        {
            // Arrange
            var mockHeatmap = new Mock<IHeatmap>(MockBehavior.Strict);
            // Strict makes sure only the setup methods are called
            mockHeatmap
                .Setup(mock => mock.ProcessCircle(It.IsAny<Point>(), It.IsAny<int>(), It.IsAny<int>()))
                .Verifiable();
            var mockHeatmapFactory = new Mock<IHeatmapFactory>();
            mockHeatmapFactory
                .Setup(mock => mock.CreateHeatmap(It.IsAny<Bounds>(), It.IsAny<int>()))
                .Returns(mockHeatmap.Object)
                .Verifiable();
            var hazardsToHeatmapResponse = new HazardsToHeatmapResponse(mockHeatmapFactory.Object);
            var hazards = new Hazard[]
            {
                new CircularHazard { Shape = "Circle" }
            };

            // Act
            hazardsToHeatmapResponse.ConvertToHeatmap(It.IsAny<Bounds>(), It.IsAny<int>(), hazards);

            // Assert
            mockHeatmapFactory.Verify();
            mockHeatmap.Verify();
        }

        [Fact]
        public void ConvertToHeatmap_CallsProcessPolygon_WhenPolygonIsPresentInHazards()
        {
            // Arrange
            var mockHeatmap = new Mock<IHeatmap>(MockBehavior.Strict);
            // Strict makes sure only the setup methods are called
            mockHeatmap
                .Setup(mock => mock.ProcessPolygon(It.IsAny<Polygon>(), It.IsAny<int>()))
                .Verifiable();
            var mockHeatmapFactory = new Mock<IHeatmapFactory>();
            mockHeatmapFactory
                .Setup(mock => mock.CreateHeatmap(It.IsAny<Bounds>(), It.IsAny<int>()))
                .Returns(mockHeatmap.Object)
                .Verifiable();
            var hazardsToHeatmapResponse = new HazardsToHeatmapResponse(mockHeatmapFactory.Object);
            var hazards = new Hazard[]
            {
                new PolygonalHazard { Shape = "Polygon" }
            };

            // Act
            hazardsToHeatmapResponse.ConvertToHeatmap(It.IsAny<Bounds>(), It.IsAny<int>(), hazards);

            // Assert
            mockHeatmapFactory.Verify();
            mockHeatmap.Verify();
        }

        [Fact]
        public void ConvertToHeatmap_ReturnsTheHeatmapOnWhichProcessMethodsAreCalled()
        {
            // Arrange
            var mockHeatmap = new Mock<IHeatmap>();
            var mockHeatmapObject = mockHeatmap.Object;
            var mockHeatmapFactory = new Mock<IHeatmapFactory>();
            mockHeatmapFactory
                .Setup(mock => mock.CreateHeatmap(It.IsAny<Bounds>(), It.IsAny<int>()))
                .Returns(mockHeatmapObject)
                .Verifiable();
            var hazardsToHeatmapResponse = new HazardsToHeatmapResponse(mockHeatmapFactory.Object);

            // Act
            var heatmap = hazardsToHeatmapResponse.ConvertToHeatmap(
                It.IsAny<Bounds>(), It.IsAny<int>(), new Hazard[] { });

            // Assert
            mockHeatmapFactory.Verify();
            Assert.Equal(mockHeatmapObject, heatmap);
        }
    }
}