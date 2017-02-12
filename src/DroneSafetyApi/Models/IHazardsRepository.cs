using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Spatial;

namespace DroneSafetyApi.Models
{
    public interface IHazardsRepository
    {
        // Keys in the return value signify the names of each relevant data source
        // Values in the return value signify the hazards intersecting in the given area
        Dictionary<string, IEnumerable<Hazard>> GetHazardsIntersectingWith(Polygon area);
    }
}
