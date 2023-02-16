using Enterprise.Solution.Data.Models.Base;

namespace Enterprise.Solution.Data.Helpers
{
    public class EntityListWithPaginationMetadata<T> where T : BaseEntity
    {
        public EntityListWithPaginationMetadata(
            IReadOnlyList<T> entities,
            PaginationMetadata paginationMetadata
            )
        {
            Entities = entities;
            PaginationMetadata = paginationMetadata;
        }

        public IReadOnlyList<T> Entities { get; set; } = null!;
        public PaginationMetadata PaginationMetadata { get; set; } = null!;
    }
}
