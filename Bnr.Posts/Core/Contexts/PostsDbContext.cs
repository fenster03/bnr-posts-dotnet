using Bnr.Posts.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bnr.Posts.Core.Contexts
{
    public class PostsDbContext : DbContext, IPostsDbContext
    {
        public PostsDbContext() : base() { }
        public PostsDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
