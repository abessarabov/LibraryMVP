namespace Library.Domain.Extentions
{
    public static class EnumerableUtils
    {
        public static IEnumerable<List<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
        {
            var chunk = new List<T>(chunkSize);
            foreach (var item in source)
            {
                chunk.Add(item);
                if (chunk.Count == chunkSize)
                {
                    yield return chunk;
                    chunk = new List<T>(chunkSize);
                }
            }
            if (chunk.Count > 0)
                yield return chunk;
        }
    }
}
