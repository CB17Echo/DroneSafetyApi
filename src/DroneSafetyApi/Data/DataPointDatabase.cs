using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Models
{
    public class DataPointDatabase : IDataPointRepository
    {
        private const string EndpointUri = "https://localhost:8081";
        private const string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private static string DatabaseName = "DataPoints";
        private static string CollectionName = "Data";

        private DocumentClient client;
        private Database database;
        private DocumentCollection collection;

        public DataPointDatabase()
        {
            client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
            database = new Database { Id = DatabaseName };
            client.CreateDatabaseIfNotExistsAsync(database).Wait();
            collection = new DocumentCollection { Id = CollectionName };
            client.CreateDocumentCollectionAsync(database.SelfLink, collection);
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

