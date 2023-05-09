using AutoMapper;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateUserForm, ApplicationUser>();
        CreateMap<ApplicationUser, UserDetails>();
        CreateMap<Room, RoomListDTO>()
            .ForMember(dest => dest.EquipmentNames, op => op.MapFrom(src => src.Equipments.Select(e => e.Name).ToList()))
            .ForMember(dest => dest.ImageURLs, op => op.MapFrom(src => src.Images.Select(e => e.ImageUrl).ToList()));
    }



}
