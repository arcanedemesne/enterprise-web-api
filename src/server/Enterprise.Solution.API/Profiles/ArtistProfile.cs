using AutoMapper;

namespace Enterprise.Solution.API.Profiles
{
    /// <summary>
    /// ArtistProfile for automapper
    /// </summary>
    public class ArtistProfile : Profile
    {
        /// <summary>
        /// Constructor for creating mapping profiles 
        /// </summary>
        public ArtistProfile()
        {
            CreateMap<Data.Entities.Artist, Models.ArtistDTO>();
            CreateMap<Models.ArtistDTO, Data.Entities.Artist>();
        }
    }

}
