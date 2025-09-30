
using Library.Domain.Entities;

namespace Library.Domain.Repositories
{
    public interface ISectionElasticWriteRepository
    {
        Task IndexListAsync<T>(IEnumerable<T> items) where T : class;
    }
}
