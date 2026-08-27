using System;

namespace BookingAPI.Services
{
    public interface IPricingService
    {
        decimal CalculateRentalPrice(decimal basePrice, DateTime startTime, int durationHours);
    }
}