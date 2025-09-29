using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Server.External.NoSql.ElasticSearch
{
    public class ElasticClientFactory : IElasticClientFactory
    {
        private readonly IConfiguration _configuration;

        public ElasticClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IElasticClient CreateClient()
        {
            var uri = new Uri(_configuration["Elastic:Url"] ?? throw new InvalidOperationException("Elastic URL is missing"));
            var settings = new ConnectionSettings(uri)
                .DefaultIndex("sections");

            return new ElasticClient(settings);
        }
    }
}
