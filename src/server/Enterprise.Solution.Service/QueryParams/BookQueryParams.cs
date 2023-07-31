namespace Enterprise.Solution.Service.QueryParams
{
    /// <summary>
    /// Book specific query params
    /// </summary>
    public class BookQueryParams
    {
        /// <summary>
        /// SearchQuery
        /// </summary>
        public string? SearchQuery { get; set; } = null;

        /// <summary>
        /// OnlyShowDeleted
        /// </summary>
        public bool? OnlyShowDeleted { get; set; } = false;

        /// <summary>
        /// Include Author
        /// </summary>
        public bool? IncludeAuthor { get; set; } = false;

        /// <summary>
        /// Include Cover
        /// </summary>
        public bool? IncludeCover { get; set; } = false;

        /// <summary>
        /// Include Cover and Artists
        /// </summary>
        public bool? IncludeCoverAndArtists { get; set; } = false;
    }
}
