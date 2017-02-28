using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Data
{
    /// <summary>
    /// The DbOptions class is a data model to represents configurations for a DocumentDb query
    /// </summary>
    public class DbOptions
    {
        /// <summary>
        /// The EndpointUri property represents the the endpoint URI of the DocumentDB database
        /// </summary>
        public string EndpointUri { get; set; }
        /// <summary>
        /// The Key property represents the the key of the DocumentDB database
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// The DbName property represents the the name of the DocumentDB database
        /// </summary>
        public string DbName { get; set; }
    }
}