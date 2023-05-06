using System.Security.Claims;

namespace Hotel.Backend.WebAPI.Helper;

internal static class ClaimsPrincipalExtensions
{
    public static string GetCurrentUserId(this ClaimsPrincipal user)
    {
        ArgumentNullException.ThrowIfNull(user);
        return user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("Current user Id is null");
    }
}
