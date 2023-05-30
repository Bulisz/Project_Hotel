﻿using System.ComponentModel.DataAnnotations;

namespace Hotel.Backend.WebAPI.Models.DTO
{
    public record UserDetails
    {
        public string Id { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        public string EmailConfirmed { get; set; } = string.Empty;
    }
}
