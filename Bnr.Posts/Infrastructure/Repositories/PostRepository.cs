using Bnr.Posts.Core.Contexts;
using Bnr.Posts.Core.Entities;
using Bnr.Posts.Infrastructure.Models.Post;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bnr.Posts.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IPostsDbContext _context;

        public PostRepository(IPostsDbContext context)
        {
            _context = context;
        }

        private IQueryable<PostModel> QueryRecords()
        {
            var qry = from p in _context.Posts
                      join u in _context.Users on p.UserId equals u.Id
                      select new PostModel
                      {
                          Id = p.Id,
                          UserId = p.UserId,
                          UserName = u.Name,
                          Title = p.Title,
                          Body = p.Body,
                          CreatedOn = p.CreatedOn,
                      };

            return qry;
        }

        public Task<IQueryable<PostModel>> SearchPosts()
        {
            var qry = QueryRecords();

            return Task.FromResult(qry);
        }

        public Task<PostModel> GetPost(Guid id)
        {
            var post = QueryRecords().SingleOrDefault(x => x.Id == id);

            return Task.FromResult(post);
        }

        public Task AddPost(PostModel post)
        {
            var entity = new Post
            {
                Id = post.Id,
                UserId = post.UserId,
                Title = post.Title,
                Body = post.Body,
                CreatedOn = post.CreatedOn,
            };

            _context.Posts.Add(entity);

            return Task.CompletedTask;
        }

        public Task<bool> DeletePost(Guid id)
        {
            var entity = _context.Posts.SingleOrDefault(x => x.Id == id);
            if (entity != null)
            {
                _context.Posts.Remove(entity);
            }

            return Task.FromResult(entity != null);
        }

        public Task<bool> CanPost(Guid userId)
        {
            var exists = _context.Users.Any(x => x.Id == userId);

            return Task.FromResult(exists);
        }

        public Task<int> Commit()
        {
            return _context.SaveChangesAsync();
        }
    }
}
