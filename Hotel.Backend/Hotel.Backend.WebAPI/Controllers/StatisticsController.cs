using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Models.DTO.StatisticsDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Hotel.Backend.WebAPI.Controllers;

[Route("hotel/[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;
    private readonly ILogger<StatisticsController> _logger;

    public StatisticsController(IStatisticsService statisticsService, ILogger<StatisticsController> logger)
    {
        _statisticsService = statisticsService;
        _logger = logger;
    }

    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
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
