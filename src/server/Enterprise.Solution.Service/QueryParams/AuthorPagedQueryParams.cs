namespace Enterprise.Solution.Service.QueryParams
{
    /// <summary>
    /// Author query params with pagination
    /// </summary>
    public class AuthorPagedQueryParams : AuthorQueryParams, IPagedQueryParams
    {
        /// <summary>
        /// SearchQuery
        /// </summary>
        public string? SearchQuery {  get; set; } = null;

        /// <summary>
        /// PageNumber
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// PageSize
        /// </summary>
        public int? PageSize { get; set; }
    }
}
