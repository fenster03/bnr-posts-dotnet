using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Bnr.Posts.Infrastructure.Common.Models
{
    public class SearchResponse<T>
    {
        public int StatusCode { get; set; } = (int)HttpStatusCode.OK;
        public T[] Records { get; set; }
        public int Total { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
