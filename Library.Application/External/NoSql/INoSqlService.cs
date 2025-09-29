
using Library.Domain.Entities;

namespace Library.Domain.External.NoSql
{
    public interface INoSqlService
    {
        Task<(IReadOnlyCollection<Section> Sections, long TotalCount)> GetPagedAsync(int page, int pageSize);

        Task PushAsync(Section section);
    }
}
