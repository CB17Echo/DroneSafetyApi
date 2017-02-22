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
    public class DataPointDatabase : IDataPointRepository
    {
        private static string CollectionName = "Hazards";

        private DocumentClient client;
        private Database database;
        private DocumentCollection collection;

        public DataPointDatabase(IOptions<DbOptions> config)
        {
            var dbConfig = config.Value;
            client = new DocumentClient(new Uri(dbConfig.EndpointUri), dbConfig.Key);
            database = client.CreateDatabaseIfNotExistsAsync(new Database { Id = dbConfig.DbName }).Result;
            collection = client.CreateDocumentCollectionIfNotExistsAsync(database.SelfLink, new DocumentCollection { Id = CollectionName }).Result;
        }

        public IEnumerable<DataPoint> GetDataPointsInRadius(Point location, int radius)
        {
            var query = client
                .CreateDocumentQuery<DataPoint>(collection.SelfLink, new FeedOptions { EnableScanInQuery = true })
                .Where(c => c.Location.Distance(location) < radius);
            return query;
        }
    }
}
