using Bnr.Posts.Infrastructure.Models.Post;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bnr.Posts.Infrastructure.Repositories
{
    public interface IPostRepository
    {
        Task<IQueryable<PostModel>> SearchPosts();
        Task<PostModel> GetPost(Guid id);
        Task AddPost(PostModel post);
        Task<bool> DeletePost(Guid id);

        Task<bool> CanPost(Guid userId);

        Task<int> Commit();
    }
}
