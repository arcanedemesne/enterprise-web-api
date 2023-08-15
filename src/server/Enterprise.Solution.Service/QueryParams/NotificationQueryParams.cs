namespace Enterprise.Solution.Service.QueryParams
{
    /// <summary>
    /// Notification specific query params
    /// </summary>
    public class NotificationQueryParams
    {
        /// <summary>
        /// SearchQuery
        /// </summary>
        public Guid AssignedTo { get; set; }

        /// <summary>
        /// OnlyShowDeleted
        /// </summary>
        public bool? OnlyShowDeleted { get; set; } = false;
    }
}
