using Library.Domain.Entities;

namespace Library.Domain.Repositories
{
    public interface IArticleRepository
    {
        Task<Article> AddOrUpdateAsync(long? articleId, string name, IEnumerable<string> tagNames);

        Task<Article> GetByIdAsync(long articleId);
    }
}
