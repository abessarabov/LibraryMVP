using System.Collections;

namespace Library.Server.Services
{
    public interface INormalizationService
    {
        ValueTask<List<string>> NormalizeTagList(IList<string> rawTags);
    }
}
