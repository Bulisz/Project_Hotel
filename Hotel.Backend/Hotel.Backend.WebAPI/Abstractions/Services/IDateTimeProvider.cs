namespace Hotel.Backend.WebAPI.Abstractions.Services
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}