using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Models
{
    public class HeatmapsQuery
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public double CornerOneLat { get; set; }
        public double CornerOneLon { get; set; }
        public double CornerTwoLat { get; set; }
        public double CornerTwoLon { get; set; }
        public Polygon Area
        {
            get
            {
                return new Polygon(
                    new[]
                    {
                         new Position(CornerOneLat, CornerOneLon),
                         new Position(CornerOneLat, CornerTwoLon),
                         new Position(CornerTwoLat, CornerTwoLat),
                         new Position(CornerTwoLat, CornerOneLon)
                    });
            }
        }
    }
}
