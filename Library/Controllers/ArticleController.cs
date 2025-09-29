using Library.Rest.Contracts.Article;
using Library.Server.Services;
using Library.Web;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Library.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleRest>> Get(long id)
        {
            try
            {
                var article = await _articleService.GetByIdAsync(id);
                return Ok(article);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpPost]
        public async Task<ActionResult<ArticleRest>> Create([FromBody] CreateArticleRequest request)
        {
            var article = await _articleService.CreateAsync(request);

            return CreatedAtAction(nameof(Get), new { id = article.ArticleId }, article);
        }

        [HttpPut]
        public async Task<ActionResult<ArticleRest>> Update([FromBody] UpdateArticleRequest request)
        {
            var article = await _articleService.UpdateAsync(request);

            return Ok(article);
        }

        [HttpDelete("{id}")]
        public ValueTask<ActionResult> Delete(long id)
        {
            // TODO: Add this later
            return new ValueTask<ActionResult>(NotFound(id));
        }
    }
}
