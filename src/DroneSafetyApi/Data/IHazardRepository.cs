using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Spatial;
using DroneSafetyApi.Models;


// Fix namespace in all the data classes and their Dependencies
namespace DroneSafetyApi.Data
{
    public interface IHazardRepository
    {
        IEnumerable<Hazard> GetHazardsInRadius(Point point, int radius, DateTime time);
        IEnumerable<Hazard> GetTypeHazardsInRadius(Point point, int radius, DateTime time, string type);
    }
}
