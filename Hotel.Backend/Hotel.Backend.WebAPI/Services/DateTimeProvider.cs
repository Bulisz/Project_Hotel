using Hotel.Backend.WebAPI.Abstractions.Services;

namespace Hotel.Backend.WebAPI.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
}
