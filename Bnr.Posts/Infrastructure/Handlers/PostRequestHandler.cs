using Bnr.Posts.Core.Entities;
using Bnr.Posts.Infrastructure.Common;
using Bnr.Posts.Infrastructure.Common.Models;
using Bnr.Posts.Infrastructure.Extensions;
using Bnr.Posts.Infrastructure.Models.Post;
using Bnr.Posts.Infrastructure.Repositories;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Bnr.Posts.Infrastructure.Handlers
{
    public class PostRequestHandler : IPostRequestHandler
    {
        private readonly IPostRepository _repo;

        public PostRequestHandler(IPostRepository repo)
        {
            _repo = repo;
        }

        public async Task<SearchResponse<PostModel>> HandleSearchRequest(SearchPostArgs args, CancellationToken cancellationToken)
        {
            var response = new SearchResponse<PostModel>();

            var qry = await _repo.SearchPosts();

            if (args.Filter?.UserId != null)
            {
                qry = qry.Where(x => x.UserId == args.Filter.UserId);
            }

            qry = qry.OrderByDescending(x => x.CreatedOn); //TODO: add sort parameter to search args

            response.Total = qry.Count();

            response.Skip = args.Skip.GetValueOrDefault();
            response.Take = args.Take.GetValueOrDefault();
            qry = qry.Skip(response.Skip).Take(response.Take);

            response.Records = qry.ToArray();

            return response;
        }

        public async Task<ModelResponse<PostModel>> HandleLoadRequest(LoadPostArgs args, CancellationToken cancellationToken)
        {
            var response = new ModelResponse<PostModel>();

            var post = await _repo.GetPost(args.Id);

            if (post == null)
            {
                return response.WithError(ErrorBuilders.PostNotFound);
            }

            return response.WithModel(post);
        }

        public async Task<ModelResponse<PostModel>> HandleCreateRequest(CreatePostArgs args, CancellationToken cancellationToken)
        {
            var response = new ModelResponse<PostModel>();

            if (!await _repo.CanPost(args.UserId))
            {
                return response.WithError(ErrorBuilders.UserCannotPost);
            }

            var post = new PostModel
            {
                Id = Guid.NewGuid(),
                UserId = args.UserId,
                Title = args.Title,
                Body = args.Body,
                CreatedOn = DateTime.UtcNow,
            };

            await _repo.AddPost(post);
            await _repo.Commit();

            return response.WithModel(post, (int)HttpStatusCode.Created);
        }

        public async Task<ModelResponse<PostModel>> HandleUpdateRequest(UpdatePostArgs args, CancellationToken cancellationToken)
        {
            var response = new ModelResponse<PostModel>();

            var post = await _repo.GetPost(args.Id);

            if (post == null)
            {
                return response.WithError(ErrorBuilders.PostNotFound);
            }

            post.Title = args.Title;
            post.Body = args.Body;

            await _repo.Commit();

            return response.WithModel(post);
        }

        public async Task<ModelResponse<DeletedModel<Guid>>> HandleDeleteRequest(DeletePostArgs args, CancellationToken cancellationToken)
        {
            var response = new ModelResponse<DeletedModel<Guid>>();

            var post = await _repo.GetPost(args.Id);

            if (post == null)
            {
                return response.WithError(ErrorBuilders.PostNotFound);
            }

            await _repo.DeletePost(post.Id);
            await _repo.Commit();

            return response.WithModel(new DeletedModel<Guid>
            {
                Id = post.Id,
                ResourceType = nameof(Post),
            });
        }

    }
}
