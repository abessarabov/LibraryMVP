using Library.Domain.ConnectionFactory;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Library.Server.Connections
{

    public class SqlConnectionFactory : IConnectionFactory<SqlConnection>
    {
        private readonly string? _connectionString;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new ArgumentNullException(
                                    nameof(configuration),
                                    "Connection string 'DefaultConnection' is not configured.");
        }

        public SqlConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
