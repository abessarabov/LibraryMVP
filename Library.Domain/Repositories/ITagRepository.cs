using Library.Domain.Entities;

namespace Library.Domain.Repositories
{
    public interface ITagRepository
    {
        Task<List<Tag>> ResolveTagNames(IEnumerable<long> tagIds, CancellationToken cancellationToken = default);
    }
}
