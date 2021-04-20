using Bnr.Posts.Controllers;
using Bnr.Posts.Infrastructure.Common.Models;
using Bnr.Posts.Infrastructure.Handlers;
using Bnr.Posts.Infrastructure.Models.Post;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Bnr.Posts.Test.Controllers
{
    public class PostsControllerTests
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IPostRequestHandler _requestHandler;

        private readonly PostsController _sut;

        public PostsControllerTests()
        {
            _logger = Substitute.For<ILogger<PostsController>>();
            _requestHandler = Substitute.For<IPostRequestHandler>();

            _sut = new PostsController(_logger, _requestHandler);
        }

        [Fact]
        public async Task Search_ShouldReturnResult()
        {
            // Arrange
            var args = new SearchPostArgs { };
            var response = new SearchResponse<PostModel> { };

            _requestHandler.HandleSearchRequest(args, default).Returns(Task.FromResult(response));

            // Act
            var result = await _sut.Search(args, default);

            // Assert
            result.As<ObjectResult>().Should().NotBeNull();
            result.As<ObjectResult>().StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.As<ObjectResult>().Value.Should().Be(response);
        }

        [Fact]
        public async Task Load_ShouldReturnResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var response = new ModelResponse<PostModel> { };

            _requestHandler.HandleLoadRequest(Arg.Is<LoadPostArgs>(x => x.Id == id), default).Returns(Task.FromResult(response));

            // Act
            var result = await _sut.Load(id, default);

            // Assert
            result.As<ObjectResult>().Should().NotBeNull();
            result.As<ObjectResult>().StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.As<ObjectResult>().Value.Should().Be(response);
        }

        [Fact]
        public async Task Create_ShouldReturnResult()
        {
            // Arrange
            var args = new CreatePostArgs { };
            var response = new ModelResponse<PostModel> { };

            _requestHandler.HandleCreateRequest(args, default).Returns(Task.FromResult(response));

            // Act
            var result = await _sut.Create(args, default);

            // Assert
            result.As<ObjectResult>().Should().NotBeNull();
            result.As<ObjectResult>().StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.As<ObjectResult>().Value.Should().Be(response);
        }

        [Fact]
        public async Task Update_ShouldReturnResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var args = new UpdatePostArgs { };
            var response = new ModelResponse<PostModel> { };

            _requestHandler.HandleUpdateRequest(args, default).Returns(Task.FromResult(response));

            // Act
            var result = await _sut.Update(id, args, default);

            // Assert
            args.Id.Should().Be(id);
            result.As<ObjectResult>().Should().NotBeNull();
            result.As<ObjectResult>().StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.As<ObjectResult>().Value.Should().Be(response);
        }

        [Fact]
        public async Task Delete_ShouldReturnResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var response = new ModelResponse<DeletedModel<Guid>> { };

            _requestHandler.HandleDeleteRequest(Arg.Is<DeletePostArgs>(x => x.Id == id), default).Returns(Task.FromResult(response));

            // Act
            var result = await _sut.Delete(id, default);

            // Assert
            result.As<ObjectResult>().Should().NotBeNull();
            result.As<ObjectResult>().StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.As<ObjectResult>().Value.Should().Be(response);
        }
    }
}
