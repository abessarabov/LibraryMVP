using System.Collections;
using System.Collections.Generic;

namespace Library.Server.Services
{
    public class NormalizationService : INormalizationService
    {
        public ValueTask<List<string>> NormalizeTagList(IList<string> rawTags)
        {
            List<string> normalizedTags = rawTags
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(NormalizeTag)
                .Distinct()
                .ToList();

            return new ValueTask<List<string>>(normalizedTags);
        }

        string NormalizeTag(string tag) => tag.Trim().ToLowerInvariant();
    }

}
