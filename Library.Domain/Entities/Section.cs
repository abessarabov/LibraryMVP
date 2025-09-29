namespace Library.Domain.Entities
{
    public class Section
    {
        public long SectionId { get; set; }

        public string Name { get; set; }

        public string TagsHash { get; set; }

        public bool Active { get; set; }

        public ICollection<SectionTag> SectionTags { get; set; } = new List<SectionTag>();

        public ICollection<SectionArticle> SectionArticles { get; set; } = new List<SectionArticle>();
    }
}
