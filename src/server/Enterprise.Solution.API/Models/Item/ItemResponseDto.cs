namespace Enterprise.Solution.API.Models
{
    /// <summary>
    /// a DTO representing the response as an Item 
    /// </summary>
    public class ItemResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}