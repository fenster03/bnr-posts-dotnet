using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Bnr.Posts.Infrastructure.Common
{
    public static class ErrorBuilders
    {
        public static readonly Action<ProblemDetails> PostNotFound = x =>
        {
            x.Type = "";
            x.Title = "Post not found.";
            x.Status = (int)HttpStatusCode.NotFound;
            x.Instance = "";
            x.Detail = "";
        };

        public static readonly Action<ProblemDetails> UserCannotPost = x =>
        {
            x.Type = "";
            x.Title = "User cannot post.";
            x.Status = (int)HttpStatusCode.BadRequest;
            x.Instance = "";
            x.Detail = "";
        };
    }
}
