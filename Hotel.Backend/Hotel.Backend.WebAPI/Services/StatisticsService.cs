﻿using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Models.DTO.StatisticsDTOs;

namespace Hotel.Backend.WebAPI.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IRoomRepository _roomRepository;
        public StatisticsService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;

        }

        public async Task<IEnumerable<RoomReservationPerMonthDTO>> GetRoomMonthStatAsync(int year, int month)
        {
            DateTime thisMonth = new DateTime(year, month, 1);
            double dayNumberInThisMonth = (double)DateTime.DaysInMonth(thisMonth.Year, thisMonth.Month);
            List<Room> allRooms = await _roomRepository.GetAllRoomsAsync();

            List<RoomReservationPerMonthDTO> result = new List<RoomReservationPerMonthDTO>();

            foreach (Room room in allRooms) {

                double bookedDays = 0;

                foreach (var reservation in room.Reservations)
                {
                    TimeSpan duration = reservation.BookingTo - reservation.BookingFrom;
                    int daysSpentInHotel = (int)duration.TotalDays + 1;
                    List<DateTime> bookedDaysList = new();
                    
                    for (int j = 0; j <= daysSpentInHotel-2; j++)
                    {
                        if (reservation.BookingFrom.AddDays(j).Month == month && reservation.BookingFrom.AddDays(j).Year == year) 
                        { 
                        bookedDaysList.Add(reservation.BookingFrom.AddDays(j));
                        }
                    }

                    foreach (var item in bookedDaysList)
                    {
                        bookedDays++;
                    }
                }
               
                double percentage = bookedDays / dayNumberInThisMonth * 100;

                RoomReservationPerMonthDTO data = new RoomReservationPerMonthDTO
                {
                    Id = room.Id,
                    Name = room.Name,
                    Percentage = Math.Round(percentage),
                };
                result.Add(data);
            }
            return result;
        }

        public async Task<IEnumerable<StatisticsPerYearDTO>> GetYearStatAsync(YearStatQueryDTO query) 
        {
            List<StatisticsPerYearDTO> result = new List<StatisticsPerYearDTO>();

            List<Room> choosedRooms = await _roomRepository.GetRoomsByNamesAsync(query.ChoosedRooms!);

            foreach (var room in choosedRooms)
            {
                StatisticsPerYearDTO myRoom = new StatisticsPerYearDTO
                {
                    Name = room.Name,
                    Data = new List<double>()
                };

                for (int month = 1; month <= 12; month++)
                {
                    double dayNumberInThisMonth = (double)DateTime.DaysInMonth(query.Year, month);
                    double bookedDays = 0;
                    foreach (var reservation in room.Reservations)
                    {
                        TimeSpan duration = reservation.BookingTo - reservation.BookingFrom;
                        int daysSpentInHotel = (int)duration.TotalDays + 1;
                        List<DateTime> bookedDaysList = new();

                        for (int j = 0; j <= daysSpentInHotel-2; j++)
                        {
                            if (reservation.BookingFrom.AddDays(j).Month == month && reservation.BookingFrom.AddDays(j).Year == query.Year)
                            {
                                bookedDaysList.Add(reservation.BookingFrom.AddDays(j));
                            }
                        }

                        foreach (var item in bookedDaysList)
                        {
                            bookedDays++;
                        }
                    }
                    double percentage = bookedDays / dayNumberInThisMonth * 100;
                    myRoom.Data.Add(Math.Round(percentage));
                }
                result.Add(myRoom);
            }
            return result;
        }
    }
}
