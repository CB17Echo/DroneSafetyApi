using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Models
{
    public class PolygonalHazard : Hazard
    {
        public new Polygon Location;
    }
}
