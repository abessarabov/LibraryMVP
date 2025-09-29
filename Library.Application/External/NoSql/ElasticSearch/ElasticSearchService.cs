using Library.Domain.Entities;
using Library.Domain.External.NoSql;
using Nest;

namespace Library.Server.External.NoSql.ElasticSearch
{
    public class ElasticSearchService : INoSqlService
    {

        private readonly IElasticClient _elasticClient;

        public ElasticSearchService(IElasticClientFactory factory)
        {
            _elasticClient = factory.CreateClient();
        }

        public async Task<(IReadOnlyCollection<Section> Sections, long TotalCount)> GetPagedAsync(int page, int pageSize)
        {
            var response = await _elasticClient.SearchAsync<Section>(s => s
                .From((page - 1) * pageSize)
                .Size(pageSize)
                .Sort(sort => sort.Ascending(f => f.SectionId))
                .TrackTotalHits()
            );

            if (!response.IsValid)
            {
                throw new Exception($"Elastic error: {response.OriginalException?.Message}", response.OriginalException);
            }

            return (response.Documents, response.Total);
        }

        public async Task PushAsync(Section section)
        {
            var response = await _elasticClient.IndexDocumentAsync(section);

            if (!response.IsValid)
            {
                throw new Exception($"Elastic push error: {response.OriginalException?.Message}", response.OriginalException);
            }
        }
    }
}
