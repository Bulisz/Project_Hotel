using System.Net;

namespace Hotel.Backend.WebAPI.Helpers;

public class HotelException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public HotelException(HttpStatusCode statusCode, string? message) : base(message)
    {
        StatusCode = statusCode;
    }
}
