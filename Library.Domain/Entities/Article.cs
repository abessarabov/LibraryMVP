using ProtoBuf;

namespace Library.Domain.Entities
{
    [ProtoContract]
    public class Article
    {
        [ProtoMember(1)]
        public long ArticleId { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public string TagsHash { get; set; }

        [ProtoMember(4)]
        public bool Active { get; set; }

        [ProtoMember(5)]
        public int Status { get; set; }

        [ProtoMember(6)]
        public DateTime CreatedAt { get; set; }

        [ProtoMember(7)]
        public DateTime? UpdatedAt { get; set; }

        [ProtoMember(8)]
        public ICollection<ArticleTag> ArticleTags { get; set; } = new List<ArticleTag>();
    }
}
