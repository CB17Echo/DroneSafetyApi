using System;
using Microsoft.Azure.Documents.Spatial;
using System.ComponentModel.DataAnnotations;
using DroneSafetyApi.Services;

namespace DroneSafetyApi.Models
{
    public class HeatmapsQuery
    {
        public double CornerOneLat { get; set; }
        public double CornerOneLon { get; set; }
        public double CornerTwoLat { get; set; }
        public double CornerTwoLon { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Width must be a positive integer")]
        public int Width { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Height must be a positive integer")]
        public int Height { get; set; }
        public DateTime Time { get; set; }

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
                double delX = CornerTwoLat - CornerOneLat;
                double delY = CornerTwoLon - CornerOneLon;
                return (int)(Math.Sqrt(delX * delX + delY * delY)) * HeatMap.MetresInLatDegree;
            }
        }
    }
}
