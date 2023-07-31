namespace Enterprise.Solution.Service.QueryParams
{
    /// <summary>
    /// Author specific query params
    /// </summary>
    public class AuthorQueryParams
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
        /// Include Books
        /// </summary>
        public bool? IncludeBooks { get; set; } = false;

        /// <summary>
        /// Include Books with Covers
        /// </summary>
        public bool? IncludeBooksWithCover { get; set; } = false;

        /// <summary>
        /// Include Books with Covers and Artists
        /// </summary>
        public bool? IncludeBooksWithCoverAndArtists { get; set; } = false;
    }
}
