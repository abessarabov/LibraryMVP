using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public class SectionArticle
    {
        public long ArticleId { get; set; }

        public long SectionId { get; set; }

        public bool Active { get; set; }

        public string ArticleName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime LastModified => UpdatedAt ?? CreatedAt;
    }
}
