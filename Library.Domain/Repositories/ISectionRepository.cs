using Library.Domain.Entities;

namespace Library.Domain.Repositories
{
    public interface ISectionRepository
    {
        Task<List<Section>> SectionUpsertBatch(IEnumerable<ArticleBatchItem> articleBatchItems);
    }
}
