namespace Hotel.Backend.WebAPI.Models.DTO
{
    public class NonStandardEquipmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsStandard { get; set; }
    }
}
