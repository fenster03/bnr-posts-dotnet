using Bnr.Posts.Infrastructure.Common.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Bnr.Posts.Infrastructure.Extensions
{
    public static class ModelExtensions
    {
        public static ModelResponse<T> WithModel<T>(this ModelResponse<T> response, T model, int? statusCode = null)
        {
            response.Model = model;
            response.StatusCode = statusCode ?? response.StatusCode;
            return response;
        }

        public static ModelResponse<T> WithError<T>(this ModelResponse<T> response, Action<ProblemDetails> buildError)
        {
            response.Error = new();
            buildError?.Invoke(response.Error);
            response.StatusCode = response.Error.Status ?? response.StatusCode;
            return response;
        }
    }
}
