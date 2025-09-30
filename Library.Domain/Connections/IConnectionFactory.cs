
using System.Data.Common;

namespace Library.Domain.ConnectionFactory
{
    public interface IConnectionFactory<T> where T : DbConnection
    {
        T CreateConnection();

        Task<T> CreateConnectionAsync(CancellationToken cancellationToken = default);
    }
}
