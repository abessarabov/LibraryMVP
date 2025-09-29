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
                .Sort(sort => sort
                    .Descending(f => f.SectionArticles.Count)
                )
                .TrackTotalHits()
            );

            if (!response.IsValid)
            {
                throw new Exception($"Elastic error: {response.OriginalException?.Message}", response.OriginalException);
            }

            return (response.Documents, response.Total);
        }

        public async Task<IReadOnlyCollection<SectionArticle>> GetArticlesBySectionAsync(long sectionId, int page, int pageSize)
        {
            var response = await _elasticClient.SearchAsync<Section>(s => s
                .Query(q => q.Term(t => t.SectionId, sectionId))
                .Source(sf => sf.Includes(f => f.Fields(
                    f => f.SectionArticles
                )))
            );

            var section = response.Documents.FirstOrDefault();
            if (section == null) return Array.Empty<SectionArticle>();

            return section.SectionArticles
                          .OrderByDescending(a => a.LastModified)
                          .Skip((page - 1) * pageSize)
                          .Take(pageSize)
                          .ToList();
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
