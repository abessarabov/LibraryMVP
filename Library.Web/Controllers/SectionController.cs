using Library.Domain.Entities;
using Library.Rest.Contracts.Section;
using Library.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectionsController : ControllerBase
    {
        private readonly ISectionReadService _sectionService;

        public SectionsController(ISectionReadService sectionService)
        {
            _sectionService = sectionService;
        }

        // GET /api/sections?page=1&pageSize=20
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SectionsPagedResponse>> GetPagedAsync(
            CancellationToken cancellationToken,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var (sections, totalCount) = await _sectionService.GetPagedSectionsAsync(page, pageSize, cancellationToken);
            return Ok(new SectionsPagedResponse
            {
                Sections = sections,
                TotalCount = totalCount
            });
        }

        // GET /api/sections/{id}/articles?page=1&pageSize=20
        [HttpGet("{id}/articles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ArticlesPagedResponse>> GetArticlesAsync(
            long id,
            CancellationToken cancellationToken,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var (articles, totalCount) = await _sectionService.GetArticlesBySectionAsync(id, page, pageSize, cancellationToken);
            return Ok(new ArticlesPagedResponse
            {
                Articles = articles,
                TotalCount = totalCount
            });
        }
    }

}
