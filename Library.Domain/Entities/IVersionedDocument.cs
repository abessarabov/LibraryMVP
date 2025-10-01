using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public interface IVersionedDocument
    {
        string Id { get; }

        long Version { get; }
    }
}
