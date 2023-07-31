namespace Enterprise.Solution.Service.QueryParams
{
    /// <summary>
    /// EmailSubscription query params with pagination
    /// </summary>
    public class UserPagedQueryParams : UserQueryParams, IPagedQueryParams
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
    }
}
