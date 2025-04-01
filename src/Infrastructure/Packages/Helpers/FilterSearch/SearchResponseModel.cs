namespace Helpers.FilterSearch
{
    public class SearchResponseModel<TData>
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int TotalOfPages { get; set; }
        public List<TData> Data { get; set; }
    }
}
