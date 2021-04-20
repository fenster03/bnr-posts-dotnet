using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bnr.Posts.Infrastructure.Common.Models
{
    public class DeletedModel<TId>
    {
        public TId Id { get; set; }
        public string ResourceType { get; set; }
    }
}
