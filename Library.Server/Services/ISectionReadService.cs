using Library.Rest.Contracts.Article;
using Library.Rest.Contracts.Section;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Server.Services
{
    public interface ISectionReadService
    {

        Task<(IReadOnlyCollection<SectionRest> Sections, long TotalCount)> GetPagedSectionsAsync(int page, int pageSize, CancellationToken cancellationToken = default);

        Task<(IReadOnlyCollection<ArticleRest> Articles, long TotalCount)> GetArticlesBySectionAsync(long sectionId, int page, int pageSize, CancellationToken cancellationToken = default);
    }
}
