using AutoMapper;
using Enterprise.Solution.Data.Models;

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
            CreateMap<Artist, Models.ArtistDTO_Request>();
            CreateMap<Models.ArtistDTO_Request, Artist>();

            CreateMap<Artist, Models.ArtistDTO_Response>();
            CreateMap<Models.ArtistDTO_Response, Artist>();
        }
    }

}
