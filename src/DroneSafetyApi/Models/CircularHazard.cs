using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Models
{
    public class CircularHazard : Hazard
    {
        public new Point Location { get; set; }
        public int Radius { get; set; } //Unit is meters
    }
}
