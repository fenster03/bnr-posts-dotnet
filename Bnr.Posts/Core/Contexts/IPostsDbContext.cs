using Bnr.Posts.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bnr.Posts.Core.Contexts
{
    public interface IPostsDbContext : IDisposable
    {
        DbSet<Post> Posts { get; set; }
        DbSet<User> Users { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
