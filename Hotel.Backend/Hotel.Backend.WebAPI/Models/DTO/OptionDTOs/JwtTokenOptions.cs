﻿using System.ComponentModel.DataAnnotations;

namespace Hotel.Backend.WebAPI.Models.DTO.OptionDTOs;

public record JwtTokenOptions
{
    [Required]
    public string Issuer { get; set; } = string.Empty;

    [Required]
    public string Audience { get; set; } = string.Empty;

    [Required]
    public string Key { get; set; } = string.Empty;

    [Range(1, 60)]
    public int ExpirationMinutes { get; set; } = 0;
}
