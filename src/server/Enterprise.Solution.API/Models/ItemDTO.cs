namespace Enterprise.Solution.API.Models
{
    /// <summary>
    /// a DTO representing the response shape as an Item
    /// </summary>
    public class ItemDTO
    {
        /// <summary>
        /// Id of the Item
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of the Item
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Description of the Item
        /// </summary>
        public string? Description { get; set; }
    }
}