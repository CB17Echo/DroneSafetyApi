﻿using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Models
{
    public class HeatmapsQuery
    {
        public int DecimalPlaceAccuracy { get; set; }
        public double CornerOneLat { get; set; }
        public double CornerOneLon { get; set; }
        public double CornerTwoLat { get; set; }
        public double CornerTwoLon { get; set; }
        public bool Bad
        {
            get
            {
                return (DecimalPlaceAccuracy < 0);
            }
        }
        public BoundingBox Area
        {
            get
            {
                return new BoundingBox(new Position(CornerOneLon, CornerOneLat), new Position(CornerTwoLon, CornerTwoLat));
            }
        }
    }
}
