using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public class ArticleBatchItem
    {
        public long EventId { get; set; }

        public long ArticleId { get; set; }

        public string TagsHash { get; set; }

        public string TagsConcatName { get; set; }
    }
}
