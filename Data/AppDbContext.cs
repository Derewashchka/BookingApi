using BookingAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BookingAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<ServiceOption> Services { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Сідінг початкових даних для послуг
            var projector = new ServiceOption { Id = 1, Name = "Проєктор", Price = 500 };
            var wifi = new ServiceOption { Id = 2, Name = "Wi-Fi", Price = 300 };
            var sound = new ServiceOption { Id = 3, Name = "Звук", Price = 700 };

            modelBuilder.Entity<ServiceOption>().HasData(projector, wifi, sound);

            // Сідінг залів
            var roomA = new Room { Id = 1, Name = "Зал А", Capacity = 50, BasePricePerHour = 2000 };
            var roomB = new Room { Id = 2, Name = "Зал В", Capacity = 100, BasePricePerHour = 3500 };
            var roomC = new Room { Id = 3, Name = "Зал С", Capacity = 30, BasePricePerHour = 1500 };

            modelBuilder.Entity<Room>().HasData(roomA, roomB, roomC);

            // Зв'язування залів та послуг через тіньову таблицю
            modelBuilder.Entity<Room>()
                .HasMany(r => r.AvailableServices)
                .WithMany(s => s.Rooms)
                .UsingEntity(j => j.HasData(
                    new { RoomsId = 1, AvailableServicesId = 1 },
                    new { RoomsId = 1, AvailableServicesId = 2 },
                    new { RoomsId = 2, AvailableServicesId = 1 },
                    new { RoomsId = 2, AvailableServicesId = 2 },
                    new { RoomsId = 2, AvailableServicesId = 3 },
                    new { RoomsId = 3, AvailableServicesId = 2 }
                ));
        }
    }
}