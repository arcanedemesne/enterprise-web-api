namespace Enterprise.Solution.Service.QueryParams
{
    /// <summary>
    /// Artist specific query params
    /// </summary>
    public class ArtistQueryParams
    {
        /// <summary>
        /// Include Covers
        /// </summary>
        public bool? IncludeCovers { get; set; } = false;

        /// <summary>
        /// Include Covers with Book
        /// </summary>
        public bool? IncludeCoversWithBook { get; set; } = false;

        /// <summary>
        /// Include Covers with Book and Author
        /// </summary>
        public bool? IncludeCoversWithBookAndAuthor { get; set; } = false;
    }
}
