using System;
using System.ComponentModel.DataAnnotations;

namespace Bnr.Posts.Infrastructure.Models.Post
{
    public class DeletePostArgs
    {
        [Required]
        public Guid Id { get; set; }
    }
}
