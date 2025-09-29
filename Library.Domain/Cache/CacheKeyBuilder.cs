using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Cache
{
    public static class CacheKeyBuilder
    {
        private const int Version = 1;

        public static string Article(long id) => $"v{Version}:article:{id}";
    }
}
