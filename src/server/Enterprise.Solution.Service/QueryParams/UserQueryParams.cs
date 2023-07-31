namespace Enterprise.Solution.Service.QueryParams
{
    /// <summary>
    /// User specific query params
    /// </summary>
    public class UserQueryParams
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
