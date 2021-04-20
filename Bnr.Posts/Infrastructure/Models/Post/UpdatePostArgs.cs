using System;
using System.ComponentModel.DataAnnotations;

namespace Bnr.Posts.Infrastructure.Models.Post
{
    public class UpdatePostArgs
    {
        [Required]
        public Guid Id { get; set; }
        [Required, StringLength(191)]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
    }
}
