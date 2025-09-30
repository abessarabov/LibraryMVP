
namespace Library.Rest.Contracts.Section
{
    public record SectionsPagedResponse
    {
        public IReadOnlyCollection<SectionRest> Sections { get; init; } = Array.Empty<SectionRest>();
        public long TotalCount { get; init; }
    }
}
