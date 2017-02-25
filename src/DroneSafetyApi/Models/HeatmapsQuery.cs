using System;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Models
{
    public class HeatmapsQuery
    {
        public double CornerOneLat { get; set; }
        public double CornerOneLon { get; set; }
        public double CornerTwoLat { get; set; }
        public double CornerTwoLon { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTime Time { get; set; }

        public bool Bad
        {
            get
            {
                return (Width < 0 || Height < 0);
            }
        }

        public BoundingBox Area
        {
            get
            {
                return new BoundingBox(
                    new Position(CornerOneLon, CornerOneLat),
                    new Position(CornerTwoLon, CornerTwoLat)
                    );
            }
        }

        public Point Centre
        {
            get
            {
                return new Point(
                    Area.Min.Longitude + (Area.Max.Longitude - Area.Min.Longitude) / 2,
                    Area.Min.Latitude + (Area.Max.Latitude - Area.Min.Latitude) / 2
                    );
            }
        }

        public int Radius
        {
            get
            {
                if (Radius == 0)
                {
                    double delX = CornerTwoLat - CornerOneLat;
                    double delY = CornerTwoLon - CornerOneLon;
                    Radius = (int)(Math.Sqrt(delX * delX + delY * delY)) * Services.HeatMap.MetresInLatDegree;
                }
                return Radius;
            }
            private set { Radius = value; }
        }
    }
}
