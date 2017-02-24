﻿using DroneSafetyApi.Models;
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

        public IEnumerable<T> GetHazardsInRadius<T>(Point location, int radius, string ShapeName) where T : Hazard
        {
            var query = client
                .CreateDocumentQuery<T>(collection.SelfLink, new FeedOptions { EnableScanInQuery = true })
                .Where(c => c.Location.Distance(location) < radius && c.Shape == ShapeName);
            return query;
        }
    }
}
