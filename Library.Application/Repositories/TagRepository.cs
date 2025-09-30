using Library.Domain.ConnectionFactory;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading;


namespace Library.Server.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly IConnectionFactory<SqlConnection> _connectionFactory;

        public TagRepository(IConnectionFactory<SqlConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public async Task<List<Tag>> ResolveTagNames(IEnumerable<long> tagIds, CancellationToken cancellationToken)
        {
            await using var conn = await _connectionFactory.CreateConnectionAsync(cancellationToken);

            await using var cmd = new SqlCommand("spTagGetByIds", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            // table-valued parameter
            var tagsParam = new SqlParameter("@tagIds", SqlDbType.Structured)
            {
                TypeName = "dbo.BigIntValueType",
                Value = CreateBigIntValueTypeTable(tagIds)
            };
            cmd.Parameters.Add(tagsParam);

            await using var reader = await cmd.ExecuteReaderAsync();

            return await ReadArticleFromDB(reader);
        }

        private static async Task<List<Tag>> ReadArticleFromDB(SqlDataReader reader)
        {
            List<Tag> tags = new List<Tag>();

            while (await reader.ReadAsync())
            {
                var tag = new Tag
                {
                    TagId = reader.GetInt64(reader.GetOrdinal("TagId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                };

                tags.Add(tag);
            }

          
            return tags;
        }

        private static DataTable CreateBigIntValueTypeTable(IEnumerable<long> tagIds)
        {
            var table = new DataTable();
            table.Columns.Add("Value", typeof(long));

            foreach (var id in tagIds)
            {
                table.Rows.Add(id);
            }

            return table;
        }
    }
}
