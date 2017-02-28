using DroneSafetyApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroneSafetyApi.Models;
using Moq;
using Xunit;

namespace DroneSafetyApi.UnitTests.ServicesTests
{
    class HazardsToHeatmapsResponseNoCompositionSendAllSources_ExtendedAsMock : HazardsToHeatmapsResponseNoCompositionSendAllSources
    {
        public IEnumerable<HeatmapPoint> SampleHeatmapPoints { get; private set; }

        public HazardsToHeatmapsResponseNoCompositionSendAllSources_ExtendedAsMock(IHeatmapFactory heatmapFactory) : base(heatmapFactory)
        {
            SampleHeatmapPoints = new HeatmapPoint[]
            {
                new HeatmapPoint { X=5, Y= 10, Value = 15 }
            };
        }

        public override IHeatmap ConvertToHeatmap(Bounds area, int numberLonPoints, IEnumerable<Hazard> hazards)
        {
            var mockHeatmap = new Mock<IHeatmap>(MockBehavior.Strict);
            mockHeatmap
                .Setup(mock => mock.GetHeatmapPoints())
                .Returns(SampleHeatmapPoints)
                .Verifiable();
            return mockHeatmap.Object;
        }
    }
    public class HazardsToHeatmapResponseNoCompositionSendAllSourcesTests
    {
        [Fact]
        public void ConvertToHeatmapResponse_ReturnsAnEmptyHeatmapsResponse_WhenNoHazards()
        {
            // Arrange
            var mockHeatmapFactory = new Mock<IHeatmapFactory>();
            var hazardsToHeatmapsResponse = new HazardsToHeatmapsResponseNoCompositionSendAllSources_ExtendedAsMock(mockHeatmapFactory.Object);

            // Act
            var heatmapsResponse = hazardsToHeatmapsResponse.ConvertToHeatmapResponse(It.IsAny<Bounds>(), It.IsAny<int>(), new Hazard[] { });

            // Assert
            Assert.Equal(0, heatmapsResponse.NumSources);
            Assert.Empty(heatmapsResponse.Sources);
            Assert.Empty(heatmapsResponse.Heatmaps);
        }

        [Fact]
        public void ConvertToHeatmapResponse_ReturnsAnAppropriateHeatmapsResponse_WhenHazardsPresent()
        {
            // Arrange
            var mockHeatmapFactory = new Mock<IHeatmapFactory>();
            var hazardsToHeatmapsResponse = new HazardsToHeatmapsResponseNoCompositionSendAllSources_ExtendedAsMock(mockHeatmapFactory.Object);

            var sampleSources = new string[] { "Car", "Bus", "Wifi" };
            var sampleHazards = new Hazard[sampleSources.Length+1];
            for (int i=0; i< sampleSources.Length; i++)
            {
                sampleHazards[i] = new Hazard { DataType = sampleSources[i] };
            }
            sampleHazards[sampleSources.Length] = new Hazard { DataType = sampleSources[0] };

            var sortedSampleSources = sampleSources.OrderBy(s => s);

            // Act
            var heatmapsResponse = hazardsToHeatmapsResponse.ConvertToHeatmapResponse(It.IsAny<Bounds>(), It.IsAny<int>(), sampleHazards);

            // Assert
            
            Assert.Equal(sampleSources.Length, heatmapsResponse.NumSources);
            Assert.True(sortedSampleSources.SequenceEqual(heatmapsResponse.Sources.OrderBy(s => s)));
            Assert.True(sortedSampleSources.SequenceEqual(heatmapsResponse.Heatmaps.Keys.OrderBy(s => s)));
            foreach (var keyValuePair in heatmapsResponse.Heatmaps)
            {
                Assert.Contains(sampleSources, value => value==keyValuePair.Key);
                Assert.Equal(IEnumCount(hazardsToHeatmapsResponse.SampleHeatmapPoints), IEnumCount(keyValuePair.Value));
                Assert.Equal(hazardsToHeatmapsResponse.SampleHeatmapPoints.First().X, keyValuePair.Value.First().X);
                Assert.Equal(hazardsToHeatmapsResponse.SampleHeatmapPoints.First().Y, keyValuePair.Value.First().Y);
                Assert.Equal(hazardsToHeatmapsResponse.SampleHeatmapPoints.First().Value, keyValuePair.Value.First().Value);
            }
        }

        private int IEnumCount<T>(IEnumerable<T> collection)
        {
            int count = 0;
            foreach (T item in collection)
            {
                count++;
            }
            return count;
        }
    }

}
