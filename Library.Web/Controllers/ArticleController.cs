using Azure.Core;
using Library.Rest.Contracts.Article;
using Library.Server.Services;
using Library.Web;
using Library.Web.Middleware;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Library.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(IArticleService articleService, ILogger<ArticlesController> logger)
        {
            _articleService = articleService;
            _logger = logger;
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ArticleRest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ArticleRest>> Get(long id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Begin GetById ...");

                var article = await _articleService.GetByIdAsync(id, cancellationToken);
                return Ok(article);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(ArticleRest), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ArticleRest>> Create([FromBody] CreateArticleRequest request)
        {
            _logger.LogInformation("Begin Create ...");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var article = await _articleService.CreateAsync(request);
                return CreatedAtAction(nameof(Get), new { id = article.ArticleId }, article);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ArticleRest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ArticleRest>> Update([FromBody] UpdateArticleRequest request)
        {
            _logger.LogInformation("Begin Update ...");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var article = await _articleService.UpdateAsync(request);
                return Ok(article);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ValueTask<ActionResult> Delete(long id)
        {
            _logger.LogInformation("Begin Delete ...");

            // TODO: Add this later
            return new ValueTask<ActionResult>(NotFound(id));
        }
    }
}
