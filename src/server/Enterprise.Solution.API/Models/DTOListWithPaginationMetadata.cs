using Enterprise.Solution.Data.Helpers;

namespace Enterprise.Solution.API.Models
{
    /// <summary>
    /// A special DTO for helping serialize paged data for caching purposes
    /// </summary>
    public class DTOListWithPaginationMetadata<T> where T : class
    {
        /// <summary>
        /// Constrcutor for SerializedListWithPaginationMetadata
        /// </summary>
        /// <param name="dtos"></param>
        /// <param name="paginationMetadata"></param>
        public DTOListWithPaginationMetadata(
            IReadOnlyList<T> dtos,
            PaginationMetadata paginationMetadata
        )
        {
            DTOs = dtos;
            PaginationMetadata = paginationMetadata;
        }

        /// <summary>
        /// SerializedList
        /// </summary>
        public IReadOnlyList<T> DTOs { get; set; } = null!;
        /// <summary>
        /// SerializedPaginationMetadata
        /// </summary>
        public PaginationMetadata PaginationMetadata { get; set; } = null!;
    }
}
