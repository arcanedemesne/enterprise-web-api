namespace Enterprise.Solution.Service.QueryParams
{
    /// <summary>
    /// EmailSubscription specific query params
    /// </summary>
    public class EmailSubscriptionQueryParams
    {
        /// <summary>
        /// SearchQuery
        /// </summary>
        public string? SearchQuery { get; set; } = null;

        /// <summary>
        /// OnlyShowDeleted
        /// </summary>
        public bool? OnlyShowDeleted { get; set; } = false;
    }
}
