namespace Enterprise.Solution.Service.QueryParams
{
    /// <summary>
    /// EmailSubscription query params with pagination
    /// </summary>
    public class EmailSubscriptionPagedQueryParams : IPagedQueryParams
    {
        /// <summary>
        /// SearchQuery
        /// </summary>
        public string? SearchQuery {  get; set; } = null;

        /// <summary>
        /// PageNumber
        /// </summary>
        public int? PageNumber { get; set; } = null;

        /// <summary>
        /// PageSize
        /// </summary>
        public int? PageSize { get; set; } = null;
    }
}
