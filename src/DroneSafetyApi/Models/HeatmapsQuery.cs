using System;
using Microsoft.Azure.Documents.Spatial;
using System.ComponentModel.DataAnnotations;
using DroneSafetyApi.Services;

namespace DroneSafetyApi.Models
{
    /// <summary>
    /// A class to encapsulate the query parameters supplied by the HTTP request
    /// </summary>
    public class HeatmapsQuery
    {
        /// <summary>
        /// Longitude of the first corner
        /// </summary>
        [Required]
        public double CornerOneLon { get; set; }
        /// <summary>
        /// Latitude of the first corner
        /// </summary>
        [Required]
        public double CornerOneLat { get; set; }
        /// <summary>
        /// Longitude of the second corner
        /// </summary>
        [Required]
        public double CornerTwoLon { get; set; }
        /// <summary>
        /// Latitude of the second corner
        /// </summary>
        [Required]
        public double CornerTwoLat { get; set; }
        /// <summary>
        /// Dimension of heatmap grid. Should be a positive integer.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The number of longitude points must be a positive integer")]
        public int NumberLonPoints { get; set; }
        /// <summary>
        /// Unix timestamp. Should be a nonnegative long.
        /// </summary>
        [Required]
        [Range(0, long.MaxValue, ErrorMessage = "The time must be nonnegative")]
        public long UnixTime { get; set; }
    }
}
