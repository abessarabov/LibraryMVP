using Library.Rest.Contracts.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Rest.Contracts.Section
{
    public record ArticlesPagedResponse
    {
        public IReadOnlyCollection<ArticleRest> Articles { get; init; } = Array.Empty<ArticleRest>();
        public long TotalCount { get; init; }
    }
}
