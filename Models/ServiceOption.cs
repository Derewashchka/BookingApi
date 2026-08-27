using System.Text.Json.Serialization;

namespace BookingAPI.Models
{
    public class ServiceOption
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        [JsonIgnore]
        public List<Room> Rooms { get; set; } = new();
    }
}