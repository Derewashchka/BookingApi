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

                // Пікові години (з 12:00 до 14:00): націнка 15%
                if (currentHour >= 12 && currentHour < 14)
                {
                    totalPrice += basePrice * 1.15m;
                }
                // Ранкові години (з 06:00 до 09:00): знижка 10%
                else if (currentHour >= 6 && currentHour < 9)
                {
                    totalPrice += basePrice * 0.90m;
                }
                // Вечірні години (з 18:00 до 23:00): знижка 20%
                else if (currentHour >= 18 && currentHour < 23)
                {
                    totalPrice += basePrice * 0.80m;
                }
                // Стандартні години
                else
                {
                    totalPrice += basePrice;
                }
            }

            return totalPrice;
        }
    }
}