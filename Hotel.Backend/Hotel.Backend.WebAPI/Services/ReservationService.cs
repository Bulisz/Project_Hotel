using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using System.Net;

namespace Hotel.Backend.WebAPI.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IEmailService _emailService;
    static readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    public ReservationService(IReservationRepository reservationRepository,
                              IUserRepository userRepository,
                              IRoomRepository roomRepository,
                              IEmailService emailService)
    {
        _reservationRepository = reservationRepository;
        _userRepository = userRepository;
        _roomRepository = roomRepository;
        _emailService = emailService;
    }

    public async Task<ReservationDetailsDTO> CreateReservationAsync(ReservationRequestDTO request)
    {
        if (request.BookingFrom > request.BookingTo)
        {
            List<HotelFieldError> errors = new() { new HotelFieldError("BookingTo", "A távozásnak később kell lennie, mint az érkezésnek"), new HotelFieldError("BookingFrom", "A távozásnak később kell lennie, mint az érkezésnek") };
            throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");
        }
        else if ((request.BookingFrom - DateTime.Now).TotalDays > 730)
        {
            List<HotelFieldError> errors = new() { new HotelFieldError("BookingFrom", "Csak 2 éven belül foglalhatsz.") };
            throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");
        }
        else if ((request.BookingTo - DateTime.Now).TotalDays > 730)
        {
            List<HotelFieldError> errors = new() { new HotelFieldError("BookingTo", "Csak 2 éven belül foglalhatsz.") };
            throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");
        }

        ReservationDetailsDTO response = new();

        await _semaphoreSlim.WaitAsync();
        try
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
                response = new ReservationDetailsDTO
                {
                    Id = reservation.Id,
                    RoomId = reservation.Room.Id,
                    UserId = reservation.ApplicationUser.Id,
                    BookingFrom = request.BookingFrom,
                    BookingTo = request.BookingTo,
                };
            await ReservationNotifications(user, reservation);
            }
            else
            {
                List<HotelFieldError> errors = new() { new HotelFieldError("BookingTo", "Sajnos a megadott időpontokra már foglalt a szoba"), new HotelFieldError("BookingFrom", "Sajnos a megadott időpontokra már foglalt a szoba") };
                throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");
            }
        }
        finally
        {
            _semaphoreSlim.Release();
        }

        return response;
    }

    private async Task ReservationNotifications(ApplicationUser? user, Reservation reservation)
    {
        EmailDTO confirmationEmail = _emailService.CreatingReservationConfirmationEmail(user!, reservation);
        EmailDTO notification = _emailService.CreatingNotificationOfReservation(user!, reservation);

        await _emailService.SendEmailAsync(confirmationEmail);
        await _emailService.SendEmailAsync(notification);
    }

    public async Task DeleteReservationAsync(int reservationId)
    {
        Reservation reservation = await _reservationRepository.DeleteReservationAsync(reservationId);

        EmailDTO confirmationEmail = _emailService.CreatingCancelReservationEmail(reservation);
        await _emailService.SendEmailAsync(confirmationEmail);

        EmailDTO notification = _emailService.CreatingNotificationOfCancelation(reservation);
        await _emailService.SendEmailAsync(notification);
    }

    public async Task<List<ReservationListItemDTO>> GetAllReservationsAsync()
    {
        List<Reservation> reservations = await _reservationRepository.GetAllReservationsAsync();

        List<ReservationListItemDTO> result = reservations.Select(reservation => new ReservationListItemDTO
        {
            Id = reservation.Id,
            BookingFrom = reservation.BookingFrom,
            BookingTo = reservation.BookingTo,
            UserId = reservation.ApplicationUser.Id,
            UserName = reservation.ApplicationUser.UserName!,
            RoomName = reservation.Room.Name,
            FullName = reservation.ApplicationUser.LastName + " " + reservation.ApplicationUser.FirstName,

        }).ToList();

        return result;
    }

    public async Task<List<ReservationListItemDTO>> GetMyOwnReservationsAsync(string userId)
    {
        List<Reservation> ownReservations = await _reservationRepository.GetMyReservationsAsync(userId);

        List<ReservationListItemDTO> result = ownReservations.Select(reservation => new ReservationListItemDTO
        {
            Id = reservation.Id,
            BookingFrom = reservation.BookingFrom,
            BookingTo = reservation.BookingTo,
            UserId = userId,
            RoomName = reservation.Room.Name

        }).ToList();

        return result;
    }
}
