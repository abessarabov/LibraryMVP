using Nest;

namespace Library.Domain.Entities
{
    public class Section : IVersionedDocument
    {
        public long SectionId { get; set; }

        public string Name { get; set; }

        public string TagsHash { get; set; }

        public bool Active { get; set; }

        public int ArticleCount { get; set; }

        public ICollection<SectionTag> SectionTags { get; set; } = new List<SectionTag>();

        [Ignore]
        public ICollection<SectionArticle> SectionArticles { get; set; } = new List<SectionArticle>();

        public long ArticleEventId { get; set; } = 0;

        [Ignore]
        public string Id => SectionId.ToString();

        [Ignore]
        public long Version => ArticleEventId;
    }
}
