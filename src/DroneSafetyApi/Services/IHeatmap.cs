using DroneSafetyApi.Models;
using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Services
{
    /// <summary>
    /// The IHeatmap interface is capable of mapping shape <see cref="Hazard"/>s onto heatmaps
    /// </summary>
    public interface IHeatmap
    {
        /// <summary>
        /// The GetHeatmapPoints method generates a collection of <see cref="HeatmapPoint"/>s from the current <see cref="Heatmap"/>
        /// </summary>
        /// <returns>Returns a collection of <see cref="HeatmapPoint"/>s</returns>
        IEnumerable<HeatmapPoint> GetHeatmapPoints();
        /// <summary>
        /// The ProcessPoint method maps a point hazard onto the <see cref="Heatmap"/> instance
        /// </summary>
        /// <param name="point">The point that will be mapped onto the heatmap instance</param>
        /// <param name="value">The severity of the hazard that the point represents</param>
        void ProcessPoint(Point point, int value);
        /// <summary>
        /// The ProcessCircle method maps a circular hazard onto the <see cref="Heatmap"/> instance
        /// </summary>
        /// <param name="circleCentre">The centre of the circular hazard</param>
        /// <param name="radius">The radius of the circular hazard</param>
        /// <param name="value">The severity of the hazard that the circle represents</param>
        void ProcessCircle(Point circleCentre, int radius, int value);
        /// <summary>
        /// The ProcessPolygon method maps a polygonal hazard onto the <see cref="Heatmap"/> instance
        /// </summary>
        /// <param name="polygon">The shape of the polygonal hazard</param>
        /// <param name="value">The severity of the hazard that the polygon represents</param>
        void ProcessPolygon(Polygon polygon, int value);
    }
}
