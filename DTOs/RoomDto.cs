using System.Collections.Generic;

namespace BookingAPI.DTOs
{
    public class CreateRoomDto
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public decimal BasePricePerHour { get; set; }
        public List<int> ServiceIds { get; set; } = new();
    }

    public class UpdateRoomDto
    {
        public decimal? BasePricePerHour { get; set; }
        public List<int> AddServiceIds { get; set; } = new();
    }
}