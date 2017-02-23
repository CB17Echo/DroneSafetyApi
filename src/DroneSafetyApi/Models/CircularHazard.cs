using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Models
{
    public class CircularHazard : Hazard
    {
        public int Radius { get; set; }
    }
}
