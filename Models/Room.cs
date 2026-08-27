using System.Collections.Generic;

namespace BookingAPI.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal BasePricePerHour { get; set; }

        // Відношення багато-до-багатьох для послуг
        public List<ServiceOption> AvailableServices { get; set; } = new();
    }
}