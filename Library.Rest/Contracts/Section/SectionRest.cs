using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Rest.Contracts.Section
{
    public class SectionRest
    {
        public long SectionId { get; set; }

        public string Name { get; set; }

        public int ArticleCount { get; set; }
    }
}
