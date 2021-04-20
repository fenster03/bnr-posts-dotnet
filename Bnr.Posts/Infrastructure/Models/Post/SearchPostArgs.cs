namespace Bnr.Posts.Infrastructure.Models.Post
{
    public class SearchPostArgs
    {
        public PostFilter Filter { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}
