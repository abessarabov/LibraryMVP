using Library.Domain.Cache;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Library.Rest.Contracts.Article;

namespace Library.Server.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICache _cache;

        public ArticleService(IArticleRepository articleRepository, ITagRepository tagRepository, ICache cache)
        {
            _articleRepository = articleRepository;
            _tagRepository = tagRepository;
            _cache = cache;
        }

        public Task<ArticleRest> CreateAsync(CreateArticleRequest createArticle)
        {
            return AddOrUpdateAsync(null, createArticle.Name, createArticle.TagNames);
        }

        public Task<ArticleRest> UpdateAsync(UpdateArticleRequest updateArticle)
        {
            return AddOrUpdateAsync(updateArticle.articleId, updateArticle.Name, updateArticle.TagNames);
        }

        public async Task<ArticleRest> AddOrUpdateAsync(long? articleId, string name, List<string> tagNames)
        {
            Article newArticle = await _articleRepository.AddOrUpdateAsync(articleId, name, tagNames);

            await _cache.SetAsync(CacheKeyBuilder.Article(newArticle.ArticleId), newArticle);

            return new ArticleRest() { 
                ArticleId = newArticle.ArticleId, 
                Name = newArticle.Name, 
                CreatedAt = newArticle.CreatedAt, 
                UpdatedAt = newArticle.UpdatedAt,
                TagNames = tagNames 
            };
        }

        public async Task<ArticleRest> GetByIdAsync(long artickeId)
        {
            Article? article = await _cache.GetAsync<Article>(CacheKeyBuilder.Article(artickeId));

            if (article == null)
            {
                article = await _articleRepository.GetByIdAsync(artickeId);

                await _cache.SetAsync(CacheKeyBuilder.Article(article.ArticleId), article);
            }

            ArticleRest restContract = new ArticleRest() { 
                ArticleId = article.ArticleId, 
                Name = article.Name, 
                CreatedAt = article.CreatedAt, 
                UpdatedAt = article.UpdatedAt
            };


            //TODO STORE Tag Names in cache
            List<Tag> tags = await _tagRepository.ResolveTagNames(article.ArticleTags.Select(x => x.TagId));

            restContract.TagNames = [.. tags.Select(x => x.Name)];

            return restContract;
        }


    }
}
