﻿using AutoMapper;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateUserForm, ApplicationUser>();
        CreateMap<UserDetailsDTO, UserDetails>()
            .ForMember(dest => dest.Role, op => op.MapFrom(src => src.Roles[0].ToString()))
            .ForMember(dest => dest.Id, op => op.MapFrom(src => src.User!.Id))
            .ForMember(dest => dest.UserName, op => op.MapFrom(src => src.User!.UserName))
            .ForMember(dest => dest.Email, op => op.MapFrom(src => src.User!.Email))
            .ForMember(dest => dest.FirstName, op => op.MapFrom(src => src.User!.FirstName))
            .ForMember(dest => dest.EmailConfirmed, op => op.MapFrom(src => src.User!.EmailConfirmed.ToString()))
            .ForMember(dest => dest.LastName, op => op.MapFrom(src => src.User!.LastName));
        CreateMap<Room, RoomListDTO>()
            .ForMember(dest => dest.EquipmentNames, op => op.MapFrom(src => src.Equipments.Select(e => e.Name).ToList()))
            .ForMember(dest => dest.ImageURLs, op => op.MapFrom(src => src.Images.Select(e => e.ImageUrl).ToList()));
        CreateMap<Reservation, ReservationDetailsDTO>()
            .ForMember(dest => dest.RoomId, op => op.MapFrom(src => src.Room.Id))
            .ForMember(dest => dest.UserId, op => op.MapFrom(src => src.ApplicationUser.Id));
        CreateMap<Room, RoomDetailsDTO>()
            .ForMember(dest => dest.EquipmentNames, op => op.MapFrom(src => src.Equipments.Select(e => e.Name).ToList()))
            .ForMember(dest => dest.ImageURLs, op => op.MapFrom(src => src.Images.Select(e => e.ImageUrl).ToList()));
        CreateMap<Equipment, EquipmentDTO>();
        CreateMap<EquipmentDTO, Equipment>();
        CreateMap<Post, PostDetailsDTO>();
        CreateMap<PostCreateDTO, Post>();
        CreateMap<CreateEventDTO, Event>();
        CreateMap<Event,EventDetailsDTO>();
        CreateMap<EventModifyDTO, Event>()
            .ForMember(dest => dest.Id, op => op.MapFrom(src => int.Parse(src.Id)));
        CreateMap<CreateRoomDTO, Room>()
            .ForMember(dest => dest.Price, op => op.MapFrom(src => Convert.ToDecimal(src.Price)));
        CreateMap<RoomDetailsDTO, Room>();
        CreateMap<CreateEquipmentDTO, Equipment>();
    }
}
