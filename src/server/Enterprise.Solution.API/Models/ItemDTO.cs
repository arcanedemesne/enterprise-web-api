namespace Enterprise.Solution.API.Models
{
    /// <summary>
    /// a DTO representing the response shape as an Item
    /// </summary>
    public class ItemDTO : BaseDTO
    {
        /// <summary>
        /// Name of the Item
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Description of the Item
        /// </summary>
        public string? Description { get; set; }
    }
}