using Library.Domain.Repositories;
using Library.Rest.Contracts.Article;
using Library.Rest.Contracts.Section;


namespace Library.Server.Services
{
    public class SectionReadService : ISectionReadService
    {
        private readonly ISectionElasticReadRepository _sectionElasticRepository;

        public SectionReadService(ISectionElasticReadRepository sectionElasticRepository)
        {
            _sectionElasticRepository = sectionElasticRepository;
        }

        public async Task<(IReadOnlyCollection<ArticleRest> Articles, long TotalCount)> GetArticlesBySectionAsync(long sectionId, int page, int pageSize, CancellationToken cancellationToken)
        {
            var (articles, totalCount) = await _sectionElasticRepository.GetArticlesBySectionAsync(
                sectionId, page, pageSize, cancellationToken);

            var articleRests = articles
                .Select(a => new ArticleRest
                {
                    ArticleId = a.ArticleId,
                    Name = a.ArticleName,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .ToList();

            return (articleRests, totalCount);
        }

        public async Task<(IReadOnlyCollection<SectionRest> Sections, long TotalCount)> GetPagedSectionsAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            var (sections, totalCount) = await _sectionElasticRepository.GetPagedSectionsAsync(
                page, pageSize, cancellationToken);

            var sectionRests = sections
                .Select(s => new SectionRest
                {
                    SectionId = s.SectionId,
                    Name = s.Name,
                    ArticleCount = s.ArticleCount
                })
                .ToList();

            return (sectionRests, totalCount);
        }
    }
}
