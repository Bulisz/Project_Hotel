using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Models.DTO.CalendarDTOs;
using Microsoft.Identity.Client;

namespace Hotel.Backend.WebAPI.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IReservationRepository _reservationRepository;
        public CalendarService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<List<ThisMonthCalendarDTO>> GetAllDaysOfMonthAsync()
        {
            List<Reservation> reservations = await _reservationRepository.GetAllReservationsAsync();

            List<ThisMonthCalendarDTO> allDays = new List<ThisMonthCalendarDTO>();

            //DateTime today = DateTime.Now;
            DateTime today = new DateTime(2023, 6, 6);

            for (int i = 1; i <= DateTime.DaysInMonth(today.Year, today.Month); i++)
            {
                ThisMonthCalendarDTO day = new ThisMonthCalendarDTO
                {
                    Day = new DateTime(today.Year, today.Month, i),
                    DateNumber = i,
                    WeekDayNumber = Convert.ToInt32(new DateTime(today.Year, today.Month, i).DayOfWeek)
                };

                if (day.WeekDayNumber == 0) {
                    day.WeekDayNumber = 7;
                }

                foreach (var reservation in reservations)
                {
                    TimeSpan duration = reservation.BookingTo - reservation.BookingFrom;
                    int daysSpentInHotel = (int)duration.TotalDays;
                    List<DateTime> bookedDays = new();
                    bookedDays.Add(reservation.BookingFrom);
                   
                    for (int j = 1; j <= daysSpentInHotel; j++)
                    {
                        bookedDays.Add(reservation.BookingFrom.AddDays(j));
                    }

                    foreach (var bookedDay in bookedDays)
                    {
                        if (bookedDay == day.Day && bookedDay == reservation.BookingFrom)
                        {
                            DailyReservationDTO res = new DailyReservationDTO
                            {
                                RoomNumber = reservation.Room.Id,
                                ReservationStatus = "first"
                            };
                            day.RoomStatus.Add(res);
                        }
                        else if (bookedDay == day.Day && bookedDay == reservation.BookingTo)
                        {
                            DailyReservationDTO res = new DailyReservationDTO
                            {
                                RoomNumber = reservation.Room.Id,
                                ReservationStatus = "last"
                            };
                            day.RoomStatus.Add(res);
                        }
                        else if (bookedDay == day.Day)
                        {
                            DailyReservationDTO res = new DailyReservationDTO
                            {
                                RoomNumber = reservation.Room.Id,
                                ReservationStatus = "middle"
                            };
                            day.RoomStatus.Add(res);
                        }
                    }
                }

                allDays.Add(day);
            }
            return allDays;
        }
    }
}
