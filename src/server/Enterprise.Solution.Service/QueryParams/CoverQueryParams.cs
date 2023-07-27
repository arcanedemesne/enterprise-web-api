namespace Enterprise.Solution.Service.QueryParams
{
    /// <summary>
    /// Cover specific query params
    /// </summary>
    public class CoverQueryParams
    {
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
