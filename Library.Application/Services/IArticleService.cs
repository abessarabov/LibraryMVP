using Library.Rest.Contracts.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Server.Services
{
    public interface IArticleService
    {
        Task<ArticleRest> GetByIdAsync(long id);

        Task<ArticleRest> CreateAsync(CreateArticleRequest req);

        Task<ArticleRest> UpdateAsync(UpdateArticleRequest req);
    }
}
