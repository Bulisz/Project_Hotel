﻿using AutoMapper;
using Hotel.Backend.WebAPI.Abstractions;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Repositories;
using System.Net;

namespace Hotel.Backend.WebAPI.Services
{
    public class ReservationService
    {
        private readonly IMapper _mapper;
        private readonly ReservationRepository _reservationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;

        public ReservationService(IMapper mapper, 
                                  ReservationRepository reservationRepository,
                                  IUserRepository userRepository,
                                  IRoomRepository roomRepository)
        {
            _mapper = mapper;
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
            _roomRepository = roomRepository;
        }

        public async Task<ReservationDetailsDTO> CreateReservationAsync(ReservationRequestDTO request)
        {
            if (request.BookingFrom < request.BookingTo)
            {
                UserDetailsDTO? userDTO = await _userRepository.GetUserByIdAsync(request.UserId);
                ApplicationUser? user = userDTO.User;

                Room? room = await _roomRepository.GetRoomByIdAsync(request.RoomId);

                Reservation newReservation = new Reservation
                {
                    ApplicationUser = user!,
                    BookingFrom = request.BookingFrom,
                    BookingTo = request.BookingTo,
                    Room = room!,
                };

                Reservation reservation = await _reservationRepository.CreateReservationAsync(newReservation);
                ReservationDetailsDTO response = new ReservationDetailsDTO
                {
                    Id = reservation.Id,
                    RoomId = reservation.Room.Id,
                    UserId = reservation.ApplicationUser.Id,
                    BookingFrom = request.BookingFrom,
                    BookingTo = request.BookingTo,
                };

                return response;

            }

            List<HotelFieldError> errors = new() { new HotelFieldError("Foglalási időtartam", "A távozásnak később kell lennie, mint az érkezésnek"), };
            throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");

        }
    }
}
