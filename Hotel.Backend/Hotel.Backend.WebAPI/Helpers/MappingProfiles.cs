﻿using AutoMapper;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using System.Security;

namespace Hotel.Backend.WebAPI.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateUserForm, ApplicationUser>();
        CreateMap<UserDetailsDTO, UserDetails>()
            .ForMember(dest => dest.Role, op => op.MapFrom(src => src.Roles[0].ToString()));
        CreateMap<Room, RoomListDTO>()
            .ForMember(dest => dest.EquipmentNames, op => op.MapFrom(src => src.Equipments.Select(e => e.Name).ToList()))
            .ForMember(dest => dest.ImageURLs, op => op.MapFrom(src => src.Images.Select(e => e.ImageUrl).ToList()));
        CreateMap<Reservation, ReservationDetailsDTO>()
            .ForMember(dest => dest.RoomId, op => op.MapFrom(src => src.Room.Id))
            .ForMember(dest => dest.UserId, op => op.MapFrom(src => src.ApplicationUser.Id));
        CreateMap<Room, RoomDetailsDTO>()
            .ForMember(dest => dest.EquipmentNames, op => op.MapFrom(src => src.Equipments.Select(e => e.Name).ToList()))
            .ForMember(dest => dest.ImageURLs, op => op.MapFrom(src => src.Images.Select(e => e.ImageUrl).ToList()));
        CreateMap<Equipment, NonStandardEquipmentDTO>();
        CreateMap<Post, PostDetailsDTO>();
        CreateMap<PostCreateDTO, Post>();
    }
}
