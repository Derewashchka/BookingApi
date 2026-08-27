using System;
using System.Collections.Generic;

namespace BookingAPI.DTOs
{
    public class CreateReservationDto
    {
        public int RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationHours { get; set; }
        public List<int> SelectedServiceIds { get; set; } = new();
    }
}