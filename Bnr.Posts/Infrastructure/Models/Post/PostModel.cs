using System;

namespace Bnr.Posts.Infrastructure.Models.Post
{
    public class PostModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
