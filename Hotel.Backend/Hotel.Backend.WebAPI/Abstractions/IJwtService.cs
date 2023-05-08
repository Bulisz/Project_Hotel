﻿using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace Hotel.Backend.WebAPI.Services;

public interface IJwtService
{
    LoginResponse CreateToken(IdentityUser user, IList<string> roles);

}
