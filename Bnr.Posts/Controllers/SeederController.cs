using Bnr.Posts.Core.Contexts;
using Bnr.Posts.Core.Entities;
using Bnr.Posts.Infrastructure.Extensions;
using Bnr.Posts.Infrastructure.Handlers;
using Bnr.Posts.Infrastructure.Models.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bnr.Posts.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeederController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IPostsDbContext _context;

        public SeederController(ILogger<SeederController> logger, IPostsDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var qry = from p in _context.Posts
                      join u in _context.Users on p.UserId equals u.Id
                      select new { post = p, user = u };

            var userPosts = qry.ToList()
                .GroupBy(x => x.user)
                .Select(x => new
                {
                    user = x.Key,
                    posts = x.Select(x1 => x1.post).OrderByDescending(x1 => x1.CreatedOn),
                });

            return Ok(userPosts);
        }

        [HttpPost]
        public async Task<IActionResult> Seed(CancellationToken cancellationToken)
        {
            _context.Posts.RemoveRange(_context.Posts);
            await _context.SaveChangesAsync(default);
            _context.Users.RemoveRange(_context.Users);
            await _context.SaveChangesAsync(default);

            var rand = new Random();
            var numUsers = rand.Next(10, 20);
            for (var i = 0; i < numUsers; i++)
            {
                var user = GenFu.GenFu.New<User>();
                _context.Users.Add(user);
                await _context.SaveChangesAsync(default);

                var numPosts = rand.Next(20);
                if (numPosts % 3 == 0) continue;

                for (var j = 0; j < numPosts; j++)
                {
                    var post = GenFu.GenFu.New<Post>();
                    post.UserId = user.Id;
                    _context.Posts.Add(post);
                }
                await _context.SaveChangesAsync(default);
            }

            return Ok(new
            {
                posts = _context.Posts.Count(),
                users = _context.Users.Count(),
            });
        }
    }
}
