using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Rest.Contracts.Article
{
    public record UpdateArticleRequest(long articleId, string Name, List<string> TagNames);
}
