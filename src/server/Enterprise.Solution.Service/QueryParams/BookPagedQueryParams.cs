namespace Enterprise.Solution.Service.QueryParams
{
    /// <summary>
    /// Book query params with pagination
    /// </summary>
    public class BookPagedQueryParams : BookQueryParams, IPagedQueryParams
    {
        /// <summary>
        /// PageNumber
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// PageSize
        /// </summary>
        public int? PageSize { get; set; }
        /// <summary>
        /// OrderBy
        /// </summary>
        public string? OrderBy { get; set; } = null;
        /// <summary>
        /// SearchQuery
        /// </summary>
        public string? SearchQuery { get; set; } = null;
    }
}
