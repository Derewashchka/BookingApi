using BookingAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AnalyticsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetBusinessReport()
        {
            var totalRevenue = await _context.Reservations.SumAsync(r => r.TotalPrice);

            var roomPopularity = await _context.Reservations
                .GroupBy(r => r.Room.Name)
                .Select(g => new { RoomName = g.Key, BookingsCount = g.Count() })
                .OrderByDescending(r => r.BookingsCount)
                .ToListAsync();

            return Ok(new
            {
                TotalRevenue = totalRevenue,
                PopularRooms = roomPopularity
            });
        }
    }
}