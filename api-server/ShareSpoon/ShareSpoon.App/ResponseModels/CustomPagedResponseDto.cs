namespace ShareSpoon.App.ResponseModels
{
    public class CustomPagedResponseDto<T>
    {
        public List<T> Elements { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public int ResultsCount { get; set; }

        public CustomPagedResponseDto(List<T> elements, int pageIndex, int totalPages, int resultsCount)
        {
            Elements = elements;
            PageIndex = pageIndex;
            TotalPages = totalPages;
            ResultsCount = resultsCount;
        }
    }
}
