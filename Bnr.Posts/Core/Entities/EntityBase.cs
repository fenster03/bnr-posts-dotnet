using System.ComponentModel.DataAnnotations;

namespace Bnr.Posts.Core.Entities
{
    public abstract class EntityBase<TId>
    {
        [Key]
        public TId Id { get; set; }
    }
}
