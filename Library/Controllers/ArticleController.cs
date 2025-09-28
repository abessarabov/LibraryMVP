using Library.Web;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : ControllerBase
    {

        private readonly ILogger<ArticleController> _logger;

        public ArticleController(ILogger<ArticleController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetArticle")]
        public Task Get()
        {
            throw new NotImplementedException();
        }
    }
}
