using Library.Domain.ConnectionFactory;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading;

namespace Library.Server.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly IConnectionFactory<SqlConnection> _connectionFactory;

        public ArticleRepository(IConnectionFactory<SqlConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Article> GetByIdAsync(long articleId, CancellationToken cancellationToken)
        {
            await using var conn = await _connectionFactory.CreateConnectionAsync(cancellationToken);

            await using var cmd = new SqlCommand("spArticleGetById", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@articleId", articleId);

            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

            return await ReadArticleFromDB(reader);
        }

        public async Task<Article> AddOrUpdateAsync(long? articleId, string name, IEnumerable<string> tagNames)
        {
            await using var conn = await _connectionFactory.CreateConnectionAsync();

            await using var cmd = new SqlCommand("spArticleUpsert", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (articleId != null)
            {
                cmd.Parameters.AddWithValue("@articleId", articleId);
            }

            cmd.Parameters.AddWithValue("@name", name);

            // table-valued parameter
            var tagsParam = new SqlParameter("@tags", SqlDbType.Structured)
            {
                TypeName = "dbo.TagList",
                Value = CreateTagListTable(tagNames)
            };

            cmd.Parameters.Add(tagsParam);

            await using var reader = await cmd.ExecuteReaderAsync();

            return await ReadArticleFromDB(reader);
        }

        private static async Task<Article> ReadArticleFromDB(SqlDataReader reader)
        {
            Article? article = null;
            if (await reader.ReadAsync())
            {
                article = new Article
                {
                    ArticleId = reader.GetInt64(reader.GetOrdinal("ArticleId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Active = reader.GetBoolean(reader.GetOrdinal("Active")),
                    Status = reader.GetByte(reader.GetOrdinal("Status")),
                    TagsHash = reader.GetString(reader.GetOrdinal("TagsHash")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                    UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                    ArticleTags = new List<ArticleTag>()
                };
            }

            if (article != null && await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    var tag = new ArticleTag
                    {
                        ArticleId = article.ArticleId,
                        TagId = reader.GetInt64(reader.GetOrdinal("TagId")),
                        OrderNum = reader.GetInt32(reader.GetOrdinal("OrderNum")),
                    };

                    article?.ArticleTags.Add(tag);
                }
            }

            return article!;
        }

        private static DataTable CreateTagListTable(IEnumerable<string> tagNames)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(long));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("OrderNum", typeof(int));

            int orderNum = 0;

            foreach (var name in tagNames)
            {
                table.Rows.Add(DBNull.Value, name, orderNum++);
            }

            return table;
        }
    }
}
