using BookingAPI.Data;
using BookingAPI.DTOs;
using BookingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoomsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDto dto)
        {
            var services = await _context.Services.Where(s => dto.ServiceIds.Contains(s.Id)).ToListAsync();

            var room = new Room
            {
                Name = dto.Name,
                Capacity = dto.Capacity,
                BasePricePerHour = dto.BasePricePerHour,
                AvailableServices = services
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Зал успішно створено", RoomId = room.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] UpdateRoomDto dto)
        {
            var room = await _context.Rooms.Include(r => r.AvailableServices).FirstOrDefaultAsync(r => r.Id == id);
            if (room == null) return NotFound("Зал не знайдено");

            if (dto.BasePricePerHour.HasValue)
            {
                room.BasePricePerHour = dto.BasePricePerHour.Value;
            }

            if (dto.AddServiceIds.Any())
            {
                var newServices = await _context.Services.Where(s => dto.AddServiceIds.Contains(s.Id)).ToListAsync();
                foreach (var service in newServices)
                {
                    if (!room.AvailableServices.Any(s => s.Id == service.Id))
                    {
                        room.AvailableServices.Add(service);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Зал успішно оновлено" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound("Зал не знайдено");

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Зал успішно видалено" });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchAvailableRooms([FromQuery] DateTime date, [FromQuery] int durationHours, [FromQuery] int capacity)
        {
            var endTime = date.AddHours(durationHours);

            // Знаходимо ID залів, які вже заброньовані на цей час
            var bookedRoomIds = await _context.Reservations
                .Where(r => r.StartTime < endTime && r.StartTime.AddHours(r.DurationHours) > date)
                .Select(r => r.RoomId)
                .ToListAsync();

            var availableRooms = await _context.Rooms
                .Include(r => r.AvailableServices)
                .Where(r => r.Capacity >= capacity && !bookedRoomIds.Contains(r.Id))
                .ToListAsync();

            return Ok(availableRooms);
        }
    }
}