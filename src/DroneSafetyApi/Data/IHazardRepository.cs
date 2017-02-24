using System.Collections.Generic;
using Microsoft.Azure.Documents.Spatial;
using DroneSafetyApi.Models;


// Fix namespace in all the data classes and their Dependencies
namespace DroneSafetyApi.Data
{
    public interface IHazardRepository
    {
        IEnumerable<T> GetHazardsInRadius<T>(Point location, int radius, string ShapeName) where T : Hazard;
    }
}
