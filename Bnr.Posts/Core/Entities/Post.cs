using System;
using System.ComponentModel.DataAnnotations;

namespace Bnr.Posts.Core.Entities
{
    public class Post : EntityBase<Guid>
    {
        public Guid UserId { get; set; }
        //public virtual User User { get; set; }
        [StringLength(191)]
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
