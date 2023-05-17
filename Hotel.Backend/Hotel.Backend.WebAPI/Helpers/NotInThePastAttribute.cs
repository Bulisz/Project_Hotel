using System.ComponentModel.DataAnnotations;

namespace Hotel.Backend.WebAPI.Helpers
{
    public class NotInThePastAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime dateTime)
            {
                return dateTime > DateTime.Now;
            }
            return false;
        }
    }
}
