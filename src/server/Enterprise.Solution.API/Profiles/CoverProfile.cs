using AutoMapper;
using Enterprise.Solution.Data.Models;

namespace Enterprise.Solution.API.Profiles
{
    /// <summary>
    /// CoverProfile for automapper
    /// </summary>
    public class CoverProfile : Profile
    {
        /// <summary>
        /// Constructor for creating mapping profiles 
        /// </summary>
        public CoverProfile()
        {
            CreateMap<Cover, Models.CoverDTO>();
            CreateMap<Models.CoverDTO, Cover>();
        }
    }

}
