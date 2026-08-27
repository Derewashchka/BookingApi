using BookingAPI.Data;
using BookingAPI.DTOs;
using BookingAPI.Models;
using BookingAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPricingService _pricingService;

        public ReservationsController(AppDbContext context, IPricingService pricingService)
        {
            _context = context;
            _pricingService = pricingService;
        }

        [HttpPost]
        public async Task<IActionResult> BookRoom([FromBody] CreateReservationDto dto)
        {
            var room = await _context.Rooms
                .Include(r => r.AvailableServices)
                .FirstOrDefaultAsync(r => r.Id == dto.RoomId);

            if (room == null) return NotFound("Зал не знайдено");

            // Перевірка доступності
            var endTime = dto.StartTime.AddHours(dto.DurationHours);
            var isConflict = await _context.Reservations
                .AnyAsync(r => r.RoomId == dto.RoomId && r.StartTime < endTime && r.StartTime.AddHours(r.DurationHours) > dto.StartTime);

            if (isConflict) return BadRequest("Зал вже заброньовано на обраний час.");

            // Розрахунок вартості
            var baseRentalCost = _pricingService.CalculateRentalPrice(room.BasePricePerHour, dto.StartTime, dto.DurationHours);

            var selectedServices = room.AvailableServices
                .Where(s => dto.SelectedServiceIds.Contains(s.Id))
                .ToList();

            var servicesCost = selectedServices.Sum(s => s.Price); // Послуги оплачуються одноразово (або можна додати множник на години)

            var reservation = new Reservation
            {
                RoomId = dto.RoomId,
                StartTime = dto.StartTime,
                DurationHours = dto.DurationHours,
                TotalPrice = baseRentalCost + servicesCost,
                SelectedServices = selectedServices
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Бронювання успішно створено",
                ReservationId = reservation.Id,
                TotalPrice = reservation.TotalPrice
            });
        }
    }
}