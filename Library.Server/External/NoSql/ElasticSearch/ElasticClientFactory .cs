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

        private const int _defaultBatchSize = 10;
        private const int _defaultDegreeOfParallelism = 2;

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

        public int GetElasticBatchSize()
        {
            var batchSizeSetting = _configuration["Elastic:BatchSize"];

            if (batchSizeSetting == null) {
                return _defaultBatchSize;
            }

            return int.TryParse(batchSizeSetting, out var batchSize) ? batchSize : _defaultBatchSize;
        }

        public int GetElasticDegreeOfParallelism()
        {
            var degreeOfParallelismSetting = _configuration["Elastic:DegreeOfParallelism"];

            if (degreeOfParallelismSetting == null)
            {
                return _defaultDegreeOfParallelism;
            }

            return int.TryParse(degreeOfParallelismSetting, out var degreeOfParallelism) ? degreeOfParallelism : _defaultDegreeOfParallelism;
        }
    }
}
