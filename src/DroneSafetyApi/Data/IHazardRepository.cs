using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Spatial;
using DroneSafetyApi.Models;


// Fix namespace in all the data classes and their Dependencies
namespace DroneSafetyApi.Data
{
    public interface IHazardRepository
    {
        IEnumerable<Hazard> GetHazardsInRadius<T>(Point point, int radius, string ShapeName, DateTime time) where T : Hazard;
    }
}
