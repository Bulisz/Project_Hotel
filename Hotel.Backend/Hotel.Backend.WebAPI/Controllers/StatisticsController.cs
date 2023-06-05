﻿using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Migrations;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Models.DTO.StatisticsDTOs;
using Hotel.Backend.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Hotel.Backend.WebAPI.Controllers
{
    [Route("hotel/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;
        private readonly ILogger<RoomsController> _logger;

        public StatisticsController(IStatisticsService statisticsService, ILogger<RoomsController> logger)
        {
            _statisticsService = statisticsService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin,Operator")]
        [HttpGet("GetRoomMonthStat/{year}/{month}")]
        public async Task<ActionResult<IEnumerable<RoomReservationPerMonthDTO>>> GetRoomMonthStat(int year, int month)
        {
            try
            {
                var roomStats = await _statisticsService.GetRoomMonthStatAsync(year, month);
                return Ok(roomStats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetYearStat")]
        public async Task<ActionResult<IEnumerable<StatisticsPerYearDTO>>> GetYearStat([FromQuery] YearStatQueryDTO query)
        {
            try
            {
                var yearStats = await _statisticsService.GetYearStatAsync(query);
                return Ok(yearStats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
