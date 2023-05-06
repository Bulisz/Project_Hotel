using System.Text.Json.Serialization;

namespace Hotel.Backend.WebAPI.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Role
{
    Admin,
    Operator,
    Guest
}
