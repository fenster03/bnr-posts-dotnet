using Bnr.Posts.Infrastructure.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bnr.Posts.Infrastructure.Extensions
{
    public static class ResponseExtensions
    {
        public static IActionResult CreateResult<T>(this ControllerBase controller, SearchResponse<T> response)
        {
            return controller.StatusCode(response.StatusCode, response);
        }

        public static IActionResult CreateResult<T>(this ControllerBase controller, ModelResponse<T> response)
        {
            return controller.StatusCode(response.StatusCode, response);
        }
    }
}
