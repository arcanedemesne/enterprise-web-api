namespace Enterprise.Solution.Service.QueryParams
{
    /// <summary>
    /// Artist query params with pagination
    /// </summary>
    public class ArtistPagedQueryParams : ArtistQueryParams, IPagedQueryParams
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
