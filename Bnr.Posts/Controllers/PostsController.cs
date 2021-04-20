using Bnr.Posts.Infrastructure.Extensions;
using Bnr.Posts.Infrastructure.Handlers;
using Bnr.Posts.Infrastructure.Models.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bnr.Posts.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IPostRequestHandler _requestHandler;

        public PostsController(ILogger<PostsController> logger, IPostRequestHandler requestHandler)
        {
            _logger = logger;
            _requestHandler = requestHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchPostArgs args, CancellationToken cancellationToken)
        {
            var response = await _requestHandler.HandleSearchRequest(args, cancellationToken);

            return this.CreateResult(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Load([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var args = new LoadPostArgs { Id = id };
            var response = await _requestHandler.HandleLoadRequest(args, cancellationToken);

            return this.CreateResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostArgs args, CancellationToken cancellationToken)
        {
            var response = await _requestHandler.HandleCreateRequest(args, cancellationToken);

            return this.CreateResult(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePostArgs args, CancellationToken cancellationToken)
        {
            args.Id = id;
            var response = await _requestHandler.HandleUpdateRequest(args, cancellationToken);

            return this.CreateResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var args = new DeletePostArgs { Id = id };
            var response = await _requestHandler.HandleDeleteRequest(args, cancellationToken);

            return this.CreateResult(response);
        }
    }
}
