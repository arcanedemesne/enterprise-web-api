using AutoMapper;

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
            CreateMap<Data.Entities.Cover, Models.CoverDTO>();
            CreateMap<Models.CoverDTO, Data.Entities.Cover>();
        }
    }

}
