using Library.Domain.ConnectionFactory;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Library.Server.Repositories
{
    public class ArticleEventRepository : IArticleEventRepository
    {
        private IConnectionFactory<SqlConnection> _connectionFactory;

        public ArticleEventRepository(IConnectionFactory<SqlConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IReadOnlyCollection<ArticleBatchItem>> GetArticlesToBeIndexedAsync(int batchSize, long lastProcessedEventId, CancellationToken cancellationToken)
        {
            await using var conn = await _connectionFactory.CreateConnectionAsync(cancellationToken);

            await using var cmd = conn.CreateCommand();

            cmd.CommandText = "spGetArticlesToBeIndexed";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@batch", batchSize);
            cmd.Parameters.AddWithValue("@lastProcessedEventId", lastProcessedEventId);

            var articles = new List<ArticleBatchItem>();

            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                var item = new ArticleBatchItem
                {
                    EventId = reader.GetInt64(reader.GetOrdinal("EventId")),
                    ArticleId = reader.GetInt64(reader.GetOrdinal("ArticleId")),
                    TagsHash = reader.GetString(reader.GetOrdinal("TagsHash")),
                    TagsConcatName = reader.GetString(reader.GetOrdinal("TagsConcatName"))
                };
                articles.Add(item);
            }

            var grouped = articles
                .GroupBy(a => a.ArticleId)
                .Select(g => g.OrderByDescending(a => a.ArticleId).First())
                .ToList();

            return grouped;
        }
    }
}
