using System;
using Microsoft.Azure.Documents.Spatial;
using System.ComponentModel.DataAnnotations;
using DroneSafetyApi.Services;

namespace DroneSafetyApi.Models
{
    /// <summary>
    /// The HeatMapsQuerty class is a model to hold the state of a query
    /// </summary>
    public class HeatmapsQuery
    {
        /// <summary>
        /// The CornerOneLon property represents the Longitude coordinate of the first corner of the queried bounding box
        /// </summary>
        public double CornerOneLon { get; set; }
        /// <summary>
        /// The CornerOneLat property represents the Latitude coordinate of the first corner of the queried bounding box
        /// </summary>
        public double CornerOneLat { get; set; }
        /// <summary>
        /// The CornerTwoLon property represents the Longitude coordinate of the second corner of the queried bounding box
        /// </summary>
        public double CornerTwoLon { get; set; }
        /// <summary>
        /// The CornerTwoLat property represents the Latitude coordinate of the second corner of the queried bounding box
        /// </summary>
        public double CornerTwoLat { get; set; }
        /// <summary>
        /// The NumberLonPoints property represents the number of points along the Longitude axis that should be measured in
        /// the queried bounding box. It must be a positive integer.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "The number of longitude points must be a positive integer")]
        public int NumberLonPoints { get; set; }
        /// <summary>
        /// The Time property represents the time of when the <see cref="Hazard"/>s  should be active
        /// </summary>
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

        /// <summary>
        /// The Area property represents the queried bounding box
        /// </summary>
        public Bounds Area
        {
            get
            {
                return new Bounds(CornerOne, CornerTwo);
            }
        }

        /// <summary>
        /// The Centre property represents the Longitude and Latitude point of the centre the queried bounding box
        /// </summary>
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

        /// <summary>
        /// The Radius property represents the radius in meters from the <see cref="Centre"/> needed to find all hazards that lie
        /// inside the queried bounding box
        /// </summary>
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
