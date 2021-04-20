using Bnr.Posts.Core.Contexts;
using Bnr.Posts.Core.Entities;
using Bnr.Posts.Infrastructure.Models.Post;
using Bnr.Posts.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Bnr.Posts.Test.Infrastructure.Repositories
{
    public class PostRepositoryTests
    {
        private readonly IPostsDbContext _context;

        private readonly PostRepository _sut;

        private static DbContextOptions DbContextOptions()
        {
            var services = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
            var optionsBuilder = new DbContextOptionsBuilder<PostsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseInternalServiceProvider(services);
            return optionsBuilder.Options;
        }

        public PostRepositoryTests()
        {
            _context = new PostsDbContext(DbContextOptions());

            _sut = new PostRepository(_context);
        }

        [Fact]
        public async Task SearchPosts_ShouldReturnRecords()
        {
            // Arrange
            var posts = new object[10].Select(x => new Post { Id = Guid.NewGuid(), UserId = Guid.NewGuid() }).ToArray();
            var users = posts.Select(x => new User { Id = x.UserId }).ToArray();
            _context.Posts.AddRange(posts);
            _context.Users.AddRange(users);
            await _context.SaveChangesAsync(default);

            // Act
            var result = (await _sut.SearchPosts()).ToArray();

            // Arrange
            result.Should().HaveCount(posts.Count());
            result.Should().Contain(x => posts.Select(p => p.Id).Contains(x.Id));
        }

        [Fact]
        public async Task GetPost_ShouldReturnPost()
        {
            // Arrange
            var id = Guid.NewGuid();
            var posts = new object[10].Select(x => new Post { Id = Guid.NewGuid(), UserId = Guid.NewGuid() }).ToArray();
            var users = posts.Select(x => new User { Id = x.UserId }).ToArray();
            _context.Posts.AddRange(posts);
            _context.Users.AddRange(users);
            var post = new Post { Id = id, UserId = Guid.NewGuid() };
            var user = new User { Id = post.UserId };
            _context.Posts.Add(post);
            _context.Users.Add(user);
            await _context.SaveChangesAsync(default);

            // Act
            var result = await _sut.GetPost(id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetPost_ShouldReturnNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var posts = new object[10].Select(x => new Post { Id = Guid.NewGuid(), UserId = Guid.NewGuid() }).ToArray();
            var users = posts.Select(x => new User { Id = x.UserId }).ToArray();
            _context.Posts.AddRange(posts);
            _context.Users.AddRange(users);
            await _context.SaveChangesAsync(default);

            // Act
            var result = await _sut.GetPost(id);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddPost_ShouldAddEntity()
        {
            // Arrange
            var model = new PostModel
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Title = "theTitle",
                Body = "theBody",
                CreatedOn = DateTime.UtcNow,
            };

            // Act
            await _sut.AddPost(model);
            await _sut.Commit();

            // Assert
            _context.Posts.Should().ContainSingle();
            _context.Posts.Should().Contain(x => x.Id == model.Id);
            var entity = _context.Posts.Single(x => x.Id == model.Id);
            entity.UserId.Should().Be(model.UserId);
            entity.Title.Should().Be(model.Title);
            entity.Body.Should().Be(model.Body);
            entity.CreatedOn.Should().Be(model.CreatedOn);
        }

        [Fact]
        public async Task DeletePost_ShouldDeleteEntity()
        {
            // Arrange
            var id = Guid.NewGuid();

            _context.Posts.AddRange(new object[10].Select(x => new Post { Id = Guid.NewGuid() }));
            _context.Posts.Add(new() { Id = id });
            await _context.SaveChangesAsync(default);

            // Act
            var result = await _sut.DeletePost(id);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeletePost_ShouldNotFindEntity()
        {
            // Arrange
            var id = Guid.NewGuid();

            _context.Posts.AddRange(new object[10].Select(x => new Post { Id = Guid.NewGuid() }));
            await _context.SaveChangesAsync(default);

            // Act
            var result = await _sut.DeletePost(id);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task CanPost_ShouldReturnTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _context.Users.AddRange(new object[10].Select(x => new User { Id = Guid.NewGuid() }));
            _context.Users.Add(new() { Id = userId });
            await _context.SaveChangesAsync(default);

            // Act
            var result = await _sut.CanPost(userId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task CanPost_ShouldReturnFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _context.Users.AddRange(new object[10].Select(x => new User { Id = Guid.NewGuid() }));
            await _context.SaveChangesAsync(default);

            // Act
            var result = await _sut.CanPost(userId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Commit_ShouldSaveChanges()
        {
            // Arrange


            // Act
            var result = await _sut.Commit();

            // Assert
            result.Should().Be(0);
        }
    }
}
