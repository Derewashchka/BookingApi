using System;
using System.Collections.Generic;

namespace BookingAPI.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }

        public DateTime StartTime { get; set; }
        public int DurationHours { get; set; }
        public decimal TotalPrice { get; set; }

        public List<ServiceOption> SelectedServices { get; set; } = new();
    }
}