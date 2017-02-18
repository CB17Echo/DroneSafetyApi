using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Models
{
    public class HeatmapsQuery
    {
        // TODO: Find a better way to select granularity
        public int Width { get; set; }
        public int Height { get; set; }
        public double CornerOneLat { get; set; }
        public double CornerOneLon { get; set; }
        public double CornerTwoLat { get; set; }
        public double CornerTwoLon { get; set; }

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
    }
}
