using Azure;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Nest;
using System.Threading;

namespace Library.Server.External.NoSql.ElasticSearch
{
    public class SectionElasticRepository : ISectionElasticReadRepository, ISectionElasticWriteRepository
    {
        private readonly IElasticClient _elasticClient;

        public SectionElasticRepository(IElasticClientFactory factory)
        {
            _elasticClient = factory.CreateClient();
        }

        public async Task<(IReadOnlyCollection<Section> Sections, long TotalCount)> GetPagedSectionsAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            var indexName = typeof(Section).Name.ToLowerInvariant();

            var response = await _elasticClient.SearchAsync<Section>(s => s
                .From((page - 1) * pageSize)
                .Size(pageSize)
                .Sort(ss => ss.Descending(f => f.ArticleCount))
                .TrackTotalHits()
                .Index(indexName),
                cancellationToken
            );

            if (!response.IsValid && response.ApiCall.HttpStatusCode != 404)
            {
                throw new Exception($"Elastic error: {response.OriginalException?.Message}", response.OriginalException);
            }

            return (response.Documents ?? [], response.Total);
        }

        public async Task<(IReadOnlyCollection<SectionArticle> Articles, long TotalCount)> GetArticlesBySectionAsync(
            long sectionId, int page, int pageSize, CancellationToken cancellationToken)
        {
            var indexName = typeof(SectionArticle).Name.ToLowerInvariant();

            var response = await _elasticClient.SearchAsync<SectionArticle>(s => s
                .Query(q => q
                    .Term(t => t.SectionId, sectionId)
                )
                .From((page - 1) * pageSize)
                .Size(pageSize)
                .Sort(ss => ss.Descending(f => f.LastModified))
                .TrackTotalHits()
                .Index(indexName),
                cancellationToken
            );

            if (!response.IsValid && response.ApiCall.HttpStatusCode != 404)
            {
                throw new Exception($"Elastic error: {response.OriginalException?.Message}", response.OriginalException);
            }

            return (response.Documents ?? [], response.Total);
        }

        public async Task IndexListAsync<T>(IEnumerable<T> items) where T : class
        {
            var indexName = typeof(T).Name.ToLowerInvariant();
            var response = await _elasticClient.IndexManyAsync(items, indexName);

            if (!response.IsValid)
            {
                throw new Exception($"Elastic error: {response.OriginalException?.Message}", response.OriginalException);
            }
        }

    }
}
