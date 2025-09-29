using ProtoBuf;

namespace Library.Domain.Entities
{
    [ProtoContract]
    public class ArticleTag
    {
        [ProtoMember(1)]
        public long ArticleId { get; set; }

        [ProtoMember(2)]
        public long TagId { get; set; }

        [ProtoMember(3)]
        public int OrderNum { get; set; }
    }
}
