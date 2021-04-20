using System;
using System.ComponentModel.DataAnnotations;

namespace Bnr.Posts.Infrastructure.Models.Post
{
    public class LoadPostArgs
    {
        [Required]
        public Guid Id { get; set; }
    }
}
