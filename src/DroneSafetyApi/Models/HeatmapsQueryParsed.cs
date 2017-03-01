using DroneSafetyApi.Models;
using DroneSafetyApi.Services;
using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Models
{
    /// <summary>
    /// A class to encapsulate the query parameters sent to the database
    /// </summary>
    public class HeatmapsQueryParsed
    {
        private Position CornerOne;
        private Position CornerTwo;

        /// <summary>
        /// Creates a new instance of the <see cref="HeatmapsQueryParsed"/> class
        /// </summary>
        /// <param name="heatmapsQuery">The querey parameters supplied by a HTTP request</param>
        public HeatmapsQueryParsed(HeatmapsQuery heatmapsQuery)
        {
            CornerOne = new Position(heatmapsQuery.CornerOneLon, heatmapsQuery.CornerOneLat);
            CornerTwo = new Position(heatmapsQuery.CornerTwoLon, heatmapsQuery.CornerTwoLat);
            NumberLonPoints = heatmapsQuery.NumberLonPoints;
            Time = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            Time = Time.AddSeconds(heatmapsQuery.UnixTime);
        }
        /// <summary>
        /// Dimension of heatmap grid. Should be a positive integer.
        /// </summary>
        public int NumberLonPoints { get; set; }
        /// <summary>
        /// Date and time of desired hazards
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// Bounding box of the query
        /// </summary>
        public Bounds Area
        {
            get
            {
                return new Bounds(CornerOne, CornerTwo);
            }
        }
        /// <summary>
        /// Centre location of the bounding box
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
        /// Radius required to capture all hazards inside the bounding box from centre
        /// </summary>
        public int Radius
        {
            get
            {
                double deltaLongitude = (CornerOne.Longitude - CornerTwo.Longitude) / 2;
                double deltaLatitude = (CornerOne.Latitude - CornerTwo.Latitude) / 2;
                return (int)(Math.Sqrt(deltaLongitude * deltaLongitude + deltaLatitude * deltaLatitude) * Heatmap.MetresInLatDegree);
            }
        }
    }
}
