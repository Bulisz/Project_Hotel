﻿namespace Hotel.Backend.WebAPI.Models.DTO;

public class AuthenticationResponse
{
    public string? Token { get; set; }

    public DateTime Expiration { get; set; }
}