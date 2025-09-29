using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Server.External.NoSql.ElasticSearch
{
    public interface IElasticClientFactory
    {
        IElasticClient CreateClient();
    }
}
