using Bnr.Posts.Infrastructure.Common.Models;
using Bnr.Posts.Infrastructure.Models.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bnr.Posts.Infrastructure.Handlers
{
    public interface IPostRequestHandler
    {
        Task<SearchResponse<PostModel>> HandleSearchRequest(SearchPostArgs args, CancellationToken cancellationToken);
        Task<ModelResponse<PostModel>> HandleLoadRequest(LoadPostArgs args, CancellationToken cancellationToken);
        Task<ModelResponse<PostModel>> HandleCreateRequest(CreatePostArgs args, CancellationToken cancellationToken);
        Task<ModelResponse<PostModel>> HandleUpdateRequest(UpdatePostArgs args, CancellationToken cancellationToken);
        Task<ModelResponse<DeletedModel<Guid>>> HandleDeleteRequest(DeletePostArgs args, CancellationToken cancellationToken);
    }
}
