using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bnr.Posts.Core.Entities
{
    public class User : EntityBase<Guid>
    {
        [StringLength(191)]
        public string Name { get; set; }
        [StringLength(191)]
        public string Email { get; set; }
        [StringLength(191)]
        public string Expertise { get; set; }
        public DateTime? CreatedOn { get; set; }

        //public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}
