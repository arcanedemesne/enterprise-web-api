namespace Enterprise.Solution.API.Helpers.QueryParams
{
    /// <summary>
    /// Representation of parameters for paged endpoints
    /// </summary>
    public interface IPagedQueryParams
    {
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
