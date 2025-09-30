using Library.Domain.ConnectionFactory;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace Library.Server.Repositories
{
    public class SectionRepository : ISectionRepository
    {
        private readonly IConnectionFactory<SqlConnection> _connectionFactory;

        public SectionRepository(IConnectionFactory<SqlConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Section>> SectionUpsertBatchAsync(IEnumerable<ArticleBatchItem> articleBatchItems)
        {
            await using var conn = await _connectionFactory.CreateConnectionAsync();

            await using var cmd = new SqlCommand("spSectionUpsertBatch", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            // table-valued parameter
            var tagsParam = new SqlParameter("@articleList", SqlDbType.Structured)
            {
                TypeName = "dbo.ArticleList",
                Value = CreateArticleListTable(articleBatchItems)
            };

            cmd.Parameters.Add(tagsParam);

            await using var reader = await cmd.ExecuteReaderAsync();

            return await ReadArticleFromDB(reader);
        }

        private static async Task<List<Section>> ReadArticleFromDB(SqlDataReader reader)
        {
            List<Section> sections = new List<Section>();

            while (await reader.ReadAsync())
            {

                Section section = new Section
                {
                    SectionId = reader.GetInt64(reader.GetOrdinal("SectionId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Active = reader.GetBoolean(reader.GetOrdinal("Active")),
                    ArticleCount = reader.GetInt32(reader.GetOrdinal("ArticleCount")),
                    TagsHash = reader.GetString(reader.GetOrdinal("TagsHash")),
                    SectionArticles = new List<SectionArticle>(),
                    SectionTags = new List<SectionTag>()
                };

                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        SectionArticle sectionArticle = new SectionArticle
                        {
                            SectionId = reader.GetInt64(reader.GetOrdinal("SectionId")),
                            ArticleId = reader.GetInt64(reader.GetOrdinal("ArticleId")),
                            ArticleName = reader.GetString(reader.GetOrdinal("ArticleName")),
                            Active = reader.GetBoolean(reader.GetOrdinal("Active")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                        };

                        section.SectionArticles.Add(sectionArticle);
                    }
                }
                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        SectionTag sectionTag = new SectionTag
                        {
                            SectionId = reader.GetInt64(reader.GetOrdinal("SectionId")),
                            TagId = reader.GetInt64(reader.GetOrdinal("Tagid")),
                            TagName = reader.GetString(reader.GetOrdinal("TagName"))

                        };

                        section.SectionTags.Add(sectionTag);
                    }
                }

                sections.Add(section);
            }

            return sections;
        }

        private static DataTable CreateArticleListTable(IEnumerable<ArticleBatchItem> articleBatchItems)
        {
            var table = new DataTable();
            table.Columns.Add("ArticleId", typeof(long));
            table.Columns.Add("TagsHash", typeof(string));
            table.Columns.Add("TagsConcatName", typeof(string));

            foreach (var item in articleBatchItems)
            {
                table.Rows.Add(item.ArticleId, item.TagsHash, item.TagsConcatName);
            }

            return table;
        }
    }
}
