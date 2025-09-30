using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Repositories
{
    public interface IArticleEventRepository
    {
        Task<IReadOnlyCollection<ArticleBatchItem>> GetArticlesToBeIndexedAsync(int batchSize, long lastProcessedEventId, CancellationToken cancellationToken = default);
    }
}
