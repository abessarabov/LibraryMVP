using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Rest.Contracts.Article
{
    public record UpdateArticleRequest(

        [Range(1, long.MaxValue)]
        long articleId,

        [Required, StringLength(256, MinimumLength = 1)]
        string Name,

        [MinLength(1, ErrorMessage = "Нужно указать хотя бы один тег")]
        List<string> TagNames
    );
}
