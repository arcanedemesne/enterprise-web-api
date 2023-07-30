using Enterprise.Solution.Data.Models.Base;

namespace Enterprise.Solution.Data.Models
{
    public class UserRole : BaseEntity
    {
        /// <summary>
        ///     Thte Title of the Role
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     The Description of the Role
        /// </summary>
        public string Description { get; set; }
    }
}
