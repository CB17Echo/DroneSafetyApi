using System;
using Microsoft.Azure.Documents.Spatial;
using System.ComponentModel.DataAnnotations;
using DroneSafetyApi.Services;

namespace DroneSafetyApi.Models
{
    public class HeatmapsQuery
    {
        public double CornerOneLon { get; set; }
        public double CornerOneLat { get; set; }
        public double CornerTwoLon { get; set; }
        public double CornerTwoLat { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The number of longitude points must be a positive integer")]
        public int NumberLonPoints { get; set; }
        public DateTime Time { get; set; }

        private Position CornerOne
        {
            get
            {
                return new Position(CornerOneLon, CornerOneLat);
            }
        }

        private Position CornerTwo
        {
            get
            {
                return new Position(CornerTwoLon, CornerTwoLat);
            }
        }

        public Bounds Area
        {
            get
            {
                return new Bounds(CornerOne, CornerTwo);
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
                double deltaLongitude = (CornerOne.Longitude - CornerTwo.Longitude)/2;
                double deltaLatitude = (CornerOne.Latitude - CornerTwo.Latitude)/2;
                return ((int) Math.Sqrt(deltaLongitude * deltaLongitude + deltaLatitude * deltaLatitude)) * Heatmap.MetresInLatDegree;
            }
        }
    }
}
