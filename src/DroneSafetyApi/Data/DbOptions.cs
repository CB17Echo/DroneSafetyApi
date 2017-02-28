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

        public string EndpointUri { get; set; }

        public string Key { get; set; }

        public string DbName { get; set; }
    }
}