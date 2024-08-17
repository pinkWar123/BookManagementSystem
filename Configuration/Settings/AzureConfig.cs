using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Configuration.Settings
{
    public class AzureConfig
    {
        public string ConnectionStrings { get; set; }
        public string BlobContainer { get; set; }
    }
}