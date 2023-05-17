using AutoMapper;
using Hotel.Backend.WebAPI.Abstractions;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using System.Net;

namespace Hotel.Backend.WebAPI.Services;

public class ReservationService : IReservationService
{
    private readonly IMapper _mapper;
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoomRepository _roomRepository;

    public ReservationService(IMapper mapper,
                              IReservationRepository reservationRepository,
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
            ApplicationUser? user = userDTO!.User;

            Room? room = await _roomRepository.GetRoomByIdAsync(request.RoomId);

            bool isRoomAvailable = room!.Reservations.All(reservation =>
            request.BookingFrom >= reservation.BookingTo || request.BookingTo <= reservation.BookingFrom);

            if (isRoomAvailable)
            {
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
            else
            {
                List<HotelFieldError> errors = new() { new HotelFieldError("BookingTo", "Sajnos a megadott időpontokra már foglalt a szoba"), new HotelFieldError("BookingFrom", "Sajnos a megadott időpontokra már foglalt a szoba") };
                throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");
            }
        }
        else
        {
            List<HotelFieldError> errors = new() { new HotelFieldError("BookingTo", "A távozásnak később kell lennie, mint az érkezésnek"), new HotelFieldError("BookingFrom", "A távozásnak később kell lennie, mint az érkezésnek") };
            throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");
        }

    }
}
