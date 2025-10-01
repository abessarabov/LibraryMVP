using Library.Domain.Entities;
using Library.Domain.Repositories;
using Library.Rest.Contracts.Article;
using Library.Rest.Contracts.Section;


namespace Library.Server.Services
{
    public class SectionWriteService : ISectionWriteService
    {
        private readonly ISectionElasticWriteRepository _sectionElasticRepository;

        public SectionWriteService(ISectionElasticWriteRepository sectionElasticRepository)
        {
            _sectionElasticRepository = sectionElasticRepository;
        }

        public async Task IndexSectionListAsync(IEnumerable<Section> sectionList)
        {
            if (sectionList == null || !sectionList.Any())
                return;

            var sectionArticleList = sectionList.SelectMany(x => x.SectionArticles);

            await _sectionElasticRepository.IndexListAsync(sectionList);
            await _sectionElasticRepository.IndexListAsync(sectionArticleList);
        }
    }
}
