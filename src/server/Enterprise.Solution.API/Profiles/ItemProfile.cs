using AutoMapper;

namespace Enterprise.Solution.API.Profiles
{
    /// <summary>
    /// ItemProfile for automapper
    /// </summary>
    public class ItemProfile : Profile
    {
        /// <summary>
        /// ItemProfile constructor for creating mapping profiles 
        /// </summary>
        public ItemProfile()
        {
            CreateMap<Data.Entities.Item, Models.ItemRequestDto>();
            CreateMap<Data.Entities.Item, Models.ItemResponseDto>();
            CreateMap<Models.ItemRequestDto, Data.Entities.Item>();
            CreateMap<Models.ItemResponseDto, Data.Entities.Item>();
        }
    }

}
