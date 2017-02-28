using DroneSafetyApi.Models;
using DroneSafetyApi.Services;
using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Models
{
    public class HeatmapsQueryParsed
    {
        private Position CornerOne;
        private Position CornerTwo;

        public HeatmapsQueryParsed(HeatmapsQuery heatmapsQuery)
        {
            CornerOne = new Position(heatmapsQuery.CornerOneLon, heatmapsQuery.CornerOneLat);
            CornerTwo = new Position(heatmapsQuery.CornerTwoLon, heatmapsQuery.CornerTwoLat);
            NumberLonPoints = heatmapsQuery.NumberLonPoints;
            Time = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            Time = Time.AddSeconds(heatmapsQuery.UnixTime);
        }

        public int NumberLonPoints { get; set; }
        public DateTime Time { get; set; }

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
                double deltaLongitude = (CornerOne.Longitude - CornerTwo.Longitude) / 2;
                double deltaLatitude = (CornerOne.Latitude - CornerTwo.Latitude) / 2;
                return ((int)Math.Sqrt(deltaLongitude * deltaLongitude + deltaLatitude * deltaLatitude)) * Heatmap.MetresInLatDegree;
            }
        }
    }
}
