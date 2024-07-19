namespace ShareSpoon.App.ResponseModels
{
    public class PagedResponseDto<T>
    {
        public List<T> Elements { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public PagedResponseDto(List<T> elements, int pageIndex, int totalPages)
        {
            Elements = elements;
            PageIndex = pageIndex;
            TotalPages = totalPages;
        }
    }
}
