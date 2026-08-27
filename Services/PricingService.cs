using System;

namespace BookingAPI.Services
{
    public class PricingService : IPricingService
    {
        public decimal CalculateRentalPrice(decimal basePrice, DateTime startTime, int durationHours)
        {
            decimal totalPrice = 0;

            for (int i = 0; i < durationHours; i++)
            {
                var currentHour = startTime.AddHours(i).Hour;

                // Peak hours (12:00 to 14:00): surcharge 15%
                if (currentHour >= 12 && currentHour < 14)
                {
                    totalPrice += basePrice * 1.15m;
                }
                // Morning hours (from 06:00 to 09:00): discount 10%
                else if (currentHour >= 6 && currentHour < 9)
                {
                    totalPrice += basePrice * 0.90m;
                }
                // Evening hours (6:00 PM to 11:00 PM): 20% discount%
                else if (currentHour >= 18 && currentHour < 23)
                {
                    totalPrice += basePrice * 0.80m;
                }
                // Standard hours
                else
                {
                    totalPrice += basePrice;
                }
            }

            return totalPrice;
        }
    }
}