using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Models
{
    public class DataPointDB
    {
        private const string EndpointUrl = "https://localhost:8081";
        private const string AuthorizationKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        private static string DatabaseName = "DataPoints";
        private static string CollectionName = "Data";


        private static DocumentClient mClient;
        private static Database mDatabase;
        private static DocumentCollection mCollection;



        public static List<DataPoint> GetDataPoints(Point location, int radius)
        {
            var query = mClient.CreateDocumentQuery<DataPoint>(mCollection.SelfLink, new FeedOptions { EnableScanInQuery = true }).Where(c => c.Location.Distance(location) < radius);
            List<DataPoint> list = new List<DataPoint>();

            foreach (DataPoint point in query)
            {
                list.Add(point);
            }
            return list;
        }

        public static async Task AddDataPoint(DataPoint datapoint)
        {
            // Can potentially have a seperate collection from each data source
            await CreateDocument(mClient, datapoint);
        }

        public static async Task AddDataPoints(List<DataPoint> datapoints)
        {
            foreach (DataPoint datapoint in datapoints)
                await AddDataPoint(datapoint);
        }

        public static void CreateDocumentClient()
        {
            mClient = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey);
            mDatabase = mClient.CreateDatabaseQuery("SELECT * FROM c WHERE c.id ='" + DatabaseName + "'").AsEnumerable().First();
            mCollection = mClient.CreateDocumentCollectionQuery(mDatabase.CollectionsLink, "SELECT * FROM c WHERE c.id = '" + CollectionName + "'").AsEnumerable().First();
        }

        private static async Task CreateDatabase(DocumentClient client)
        {
            var databaseDef = new Database { Id = "DataPoints" };
            var result = await client.CreateDatabaseAsync(databaseDef);
            var database = result.Resource;
        }
        // Create collection ... 
        private static async Task<Document> CreateDocument(DocumentClient client, object documentObject)
        {

            var result = await client.CreateDocumentAsync(mCollection.SelfLink, documentObject);
            var document = result.Resource;
            return result;
        }

    }
}

