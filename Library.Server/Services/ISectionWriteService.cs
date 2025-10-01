using Library.Domain.Entities;
using Library.Rest.Contracts.Article;
using Library.Rest.Contracts.Section;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Server.Services
{
    public interface ISectionWriteService
    {
        Task IndexSectionListAsync(IEnumerable<Section> sectionList);
    }
}
