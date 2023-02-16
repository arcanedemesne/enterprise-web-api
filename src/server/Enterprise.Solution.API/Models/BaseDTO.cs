using System.Diagnostics.CodeAnalysis;

namespace Enterprise.Solution.API.Models
{
    /// <summary>
    /// The BaseDTO
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class BaseDTO
    {
        /// <summary>
        /// Id of the dto
        /// </summary>
        public int Id { get; set; }
    }
}