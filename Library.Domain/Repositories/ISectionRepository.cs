using Library.Domain.Entities;

namespace Library.Domain.Repositories
{
    public interface ISectionRepository
    {
        Task<List<Section>> SectionUpsertBatchAsync(IEnumerable<ArticleBatchItem> articleBatchItems);
    }
}
