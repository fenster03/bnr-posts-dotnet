using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bnr.Posts.Infrastructure.Common.Models
{
    public class ModelResponse<T>
    {
        public int StatusCode { get; set; } = (int)HttpStatusCode.OK;
        public T Model { get; set; }
        public ProblemDetails Error { get; set; }
    }
}
