namespace APIGateway.Models.PostService
{
    public class PagedResponse<T>
    {
        public PagedResponse()
        {
            Items = new List<T>();
        }
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
