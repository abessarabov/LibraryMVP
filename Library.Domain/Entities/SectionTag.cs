using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public class SectionTag
    {
        public long TagId { get; set; }

        public long SectionId { get; set; }

        public string TagName { get; set; }
    }
}
