using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Models
{
    public class Bounds
    {
        public Position Min { get; private set; }
        public Position Max { get; private set; }
        public Bounds(Position cornerOne, Position cornerTwo)
        {
            Max = new Position(
                Math.Max(cornerOne.Longitude, cornerTwo.Longitude),
                Math.Max(cornerOne.Latitude, cornerTwo.Latitude));
            Min = new Position(
                Math.Min(cornerOne.Longitude, cornerTwo.Longitude),
                Math.Min(cornerOne.Latitude, cornerTwo.Latitude));
        }
    }
}
