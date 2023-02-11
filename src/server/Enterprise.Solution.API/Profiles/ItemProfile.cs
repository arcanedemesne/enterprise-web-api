using AutoMapper;

namespace Enterprise.Solution.API.Profiles
{
    /// <summary>
    /// ItemProfile for automapper
    /// </summary>
    public class ItemProfile : Profile
    {
        /// <summary>
        /// Constructor for creating mapping profiles 
        /// </summary>
        public ItemProfile()
        {
            CreateMap<Data.Entities.Item, Models.ItemDTO>();
            CreateMap<Models.ItemDTO, Data.Entities.Item>();
        }
    }

}
