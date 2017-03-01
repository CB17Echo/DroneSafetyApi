using DroneSafetyApi.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Spatial;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Data
{
    public class HazardDatabase : IHazardRepository
    {
        private static string CollectionName = "hazards";

        private DocumentClient client;
        private Database database;
        private DocumentCollection collection;

        public HazardDatabase(IOptions<DbOptions> config)
        {
            var dbConfig = config.Value;
            client = new DocumentClient(new Uri(dbConfig.EndpointUri), dbConfig.Key);
            database = client.CreateDatabaseIfNotExistsAsync(new Database { Id = dbConfig.DbName }).Result;
            collection = client.CreateDocumentCollectionIfNotExistsAsync(database.SelfLink, new DocumentCollection { Id = CollectionName }).Result;
        }

        public IEnumerable<Hazard> GetHazardsInRadius(Point location, int radius, DateTime time)
        {
            IEnumerable<Hazard> circleHazards = GetCircularHazardsInRadius(location, radius, time);
            IEnumerable<Hazard> pointHazards = GetPointHazardsInRadius(location, radius, time);
            IEnumerable<Hazard> polygonalHazards = GetPolygonalHazardsInRadius(location, radius, time);
            return circleHazards.Concat(pointHazards).Concat(polygonalHazards);
        }

        private IEnumerable<Hazard> GetShapedHazardsInRadius<T>(Point location, int radius, DateTime time, string ShapeName) where T : Hazard
        {
            var query = client
                .CreateDocumentQuery<T>(collection.SelfLink, new FeedOptions { EnableScanInQuery = true })
                .Where(c => c.Location.Distance(location) < radius && c.Shape == ShapeName
                    && c.StartTime <= time && c.EndTime >= time);
            return query;
        }

        private IEnumerable<Hazard> GetCircularHazardsInRadius(Point location, int radius, DateTime time)
        {
            return GetShapedHazardsInRadius<CircularHazard>(location, radius, time, "Circle");
        }

        private IEnumerable<Hazard> GetPointHazardsInRadius(Point location, int radius, DateTime time)
        {
            return GetShapedHazardsInRadius<PointHazard>(location, radius, time, "Point");
        }

        private IEnumerable<Hazard> GetPolygonalHazardsInRadius(Point location, int radius, DateTime time)
        {
            return GetShapedHazardsInRadius<PolygonalHazard>(location, radius, time, "Polygon");
        }
    }
}
