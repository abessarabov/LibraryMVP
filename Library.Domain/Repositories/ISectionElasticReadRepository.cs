
using Library.Domain.Entities;

namespace Library.Domain.Repositories
{
    public interface ISectionElasticReadRepository
    {
        Task<(IReadOnlyCollection<Section> Sections, long TotalCount)> GetPagedSectionsAsync(int page, int pageSize, CancellationToken cancellationToken = default);

        Task<(IReadOnlyCollection<SectionArticle> Articles, long TotalCount)> GetArticlesBySectionAsync(long sectionId, int page, int pageSize, CancellationToken cancellationToken = default);
    }
}
