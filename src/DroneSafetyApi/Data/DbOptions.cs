using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneSafetyApi.Data
{
    public class DbOptions
    {
        public string EndpointUri { get; set; }
        public string Key { get; set; }
        public string DbName { get; set; }
    }
}