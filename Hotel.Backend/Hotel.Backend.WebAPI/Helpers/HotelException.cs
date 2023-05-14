using System.Net;

namespace Hotel.Backend.WebAPI.Helpers;

public class HotelException : Exception
{
    public HttpStatusCode Status { get; }
    public List<HotelFieldError> HotelErrors { get; set;  } = new List<HotelFieldError>();

    public HotelException(HttpStatusCode status, List<HotelFieldError> hotelErrors, string? message) : base(message)
    {
        Status = status;
        HotelErrors = hotelErrors;
    }
}

public class HotelFieldError
{
    public string FieldName { get; } = string.Empty;
    public string? FieldErrorMessage { get; } = string.Empty;

    public HotelFieldError(string field, string? message)
    {
        FieldName = field;
        FieldErrorMessage = message;
    }
}

