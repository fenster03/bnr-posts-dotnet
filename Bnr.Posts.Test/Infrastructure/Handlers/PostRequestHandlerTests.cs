using Bnr.Posts.Core.Entities;
using Bnr.Posts.Infrastructure.Handlers;
using Bnr.Posts.Infrastructure.Models.Post;
using Bnr.Posts.Infrastructure.Repositories;
using FluentAssertions;
using NSubstitute;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Bnr.Posts.Test.Infrastructure.Handlers
{
    public class PostRequestHandlerTests
    {
        private readonly IPostRepository _repo;

        private readonly PostRequestHandler _sut;

        public PostRequestHandlerTests()
        {
            _repo = Substitute.For<IPostRepository>();

            _sut = new PostRequestHandler(_repo);
        }

        [Fact]
        public async Task HandleSearchRequest_ShouldReturnResponse()
        {
            // Arrange
            var args = new SearchPostArgs();

            _repo.SearchPosts().Returns(Enumerable.Empty<PostModel>().AsQueryable());

            // Assert
            var result = await _sut.HandleSearchRequest(args, default);

            // Act
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task HandleSearchRequest_ShouldReturnUserRecords()
        {
            // Arrange
            var args = new SearchPostArgs
            {
                Filter = new()
                {
                    UserId = Guid.NewGuid(),
                },
                Take = 5,
            };

            var records = new object[10].Select(x => new PostModel { Id = Guid.NewGuid() });
            records = records.Concat(new object[10].Select(x => new PostModel { Id = Guid.NewGuid(), UserId = args.Filter.UserId.Value }));

            _repo.SearchPosts().Returns(records.AsQueryable());

            // Assert
            var result = await _sut.HandleSearchRequest(args, default);

            // Act
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Total.Should().Be(10);
            result.Records.Should().HaveCount(args.Take.Value);
        }

        [Fact]
        public async Task HandleLoadRequest_ShouldReturnResponse()
        {
            // Arrange
            var id = Guid.NewGuid();
            var args = new LoadPostArgs
            {
                Id = id,
            };
            var model = new PostModel { Id = id };

            _repo.GetPost(id).Returns(model);

            // Assert
            var result = await _sut.HandleLoadRequest(args, default);

            // Act
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Model.Should().NotBeNull();
            result.Model.Id.Should().Be(id);
            result.Error.Should().BeNull();
        }

        [Fact]
        public async Task HandleLoadRequest_ShouldReturnNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var args = new LoadPostArgs
            {
                Id = id,
            };

            _repo.GetPost(id).Returns(default(PostModel));

            // Assert
            var result = await _sut.HandleLoadRequest(args, default);

            // Act
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            result.Model.Should().BeNull();
            result.Error.Should().NotBeNull();
        }

        [Fact]
        public async Task HandleCreateRequest_ShouldReturnResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var args = new CreatePostArgs
            {
                UserId = userId,
                Title = "theTitle",
                Body = "theBody",
            };

            _repo.CanPost(userId).Returns(true);

            // Assert
            var result = await _sut.HandleCreateRequest(args, default);

            // Act
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.Created);
            result.Model.Should().NotBeNull();
            result.Model.UserId.Should().Be(userId);
            result.Model.Title.Should().Be(args.Title);
            result.Model.Body.Should().Be(args.Body);
            result.Error.Should().BeNull();

            await _repo.Received().AddPost(Arg.Any<PostModel>());
            await _repo.Received().Commit();
        }

        [Fact]
        public async Task HandleCreateRequest_ShouldReturnUserCannotPost()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var args = new CreatePostArgs
            {
                UserId = userId,
            };
            _repo.CanPost(userId).Returns(false);

            // Assert
            var result = await _sut.HandleCreateRequest(args, default);

            // Act
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            result.Model.Should().BeNull();
            result.Error.Should().NotBeNull();
        }

        [Fact]
        public async Task HandleUpdateRequest_ShouldReturnResponse()
        {
            // Arrange
            var id = Guid.NewGuid();
            var args = new UpdatePostArgs
            {
                Id = id,
                Title = "theTitle",
                Body = "theBody",
            };
            var model = new PostModel { Id = id };

            _repo.GetPost(id).Returns(model);

            // Assert
            var result = await _sut.HandleUpdateRequest(args, default);

            // Act
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Model.Should().NotBeNull();
            result.Model.Id.Should().Be(id);
            result.Model.Title.Should().Be(args.Title);
            result.Model.Body.Should().Be(args.Body);
            result.Error.Should().BeNull();

            await _repo.Received().Commit();
        }

        [Fact]
        public async Task HandleUpdateRequest_ShouldReturnNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var args = new UpdatePostArgs
            {
                Id = id,
            };

            _repo.GetPost(id).Returns(default(PostModel));

            // Assert
            var result = await _sut.HandleUpdateRequest(args, default);

            // Act
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            result.Model.Should().BeNull();
            result.Error.Should().NotBeNull();
        }

        [Fact]
        public async Task HandleDeleteRequest_ShouldReturnResponse()
        {
            // Arrange
            var id = Guid.NewGuid();
            var args = new DeletePostArgs
            {
                Id = id,
            };
            var model = new PostModel { Id = id };

            _repo.GetPost(id).Returns(model);

            // Assert
            var result = await _sut.HandleDeleteRequest(args, default);

            // Act
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Model.Should().NotBeNull();
            result.Model.Id.Should().Be(id);
            result.Model.ResourceType.Should().Be(nameof(Post));
            result.Error.Should().BeNull();

            await _repo.Received().DeletePost(id);
            await _repo.Received().Commit();
        }

        [Fact]
        public async Task HandleDeleteRequest_ShouldReturnNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var args = new DeletePostArgs
            {
                Id = id,
            };

            _repo.GetPost(id).Returns(default(PostModel));

            // Assert
            var result = await _sut.HandleDeleteRequest(args, default);

            // Act
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            result.Model.Should().BeNull();
            result.Error.Should().NotBeNull();
        }
    }
}
