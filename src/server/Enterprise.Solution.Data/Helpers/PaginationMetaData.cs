namespace Enterprise.Solution.Data.Helpers
{
    public class PaginationMetadata
    {
        public int TotalItemCount { get; set; }
        public int TotalPageCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public string? OrderBy { get; set; }

        public PaginationMetadata(
            int totalItemCount,
            int pageSize,
            int currentPage,
            string? orderBy)
        {
            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            OrderBy = orderBy;
        }
    }
}
