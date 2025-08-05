using Barbershop.Data;
using Barbershop.Data.Models;
using Barbershop.Services;
using Barbershop.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbershop.Tests
{
    public class AppointmentServiceTests
    {
        private BarbershopDbContext dbContext = null!;
        private AppointmentService appointmentService = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<BarbershopDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            dbContext = new BarbershopDbContext(options);
            appointmentService = new AppointmentService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        [Test]
        public async Task AddAsync_Should_Add_Appointment_To_Database()
        {
            var service = new Service { Id = 1, Name = "Мъжко подстригване", DurationInMinutes = 30, Description = "Kaчествено мъжко подстригване" };
            await dbContext.Services.AddAsync(service);
            await dbContext.SaveChangesAsync();

            var model = new AppointmentCreateModel
            {
                UserId = "test-user",
                ServiceId = 1,
                AppointmentDate = DateTime.Now
            };

            await appointmentService.AddAsync(model);

            var appointments = await dbContext.Appointments.ToListAsync();
            Assert.That(appointments.Count, Is.EqualTo(1));
            Assert.That(appointments[0].UserId, Is.EqualTo("test-user"));
        }

        [Test]
        public async Task GetAllByUserAsync_Should_Return_User_Appointments()
        {
            var service = new Service { Id = 1, Name = "Мъжко подстригване", DurationInMinutes = 30, Description = "Kaчествено мъжко подстригване" };
            await dbContext.Services.AddAsync(service);
            await dbContext.SaveChangesAsync();

            await dbContext.Appointments.AddAsync(new Appointment
            {
                UserId = "user-1",
                ServiceId = 1,
                AppointmentDate = DateTime.Now
            });
            await dbContext.SaveChangesAsync();

            var result = await appointmentService.GetAllByUserAsync("user-1");

            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetAvailableSlotsAsync_Should_Respect_Taken_Slots()
        {
            var service = new Service { Id = 2, Name = "Мъжко подстригване", DurationInMinutes = 30, Description = "Kaчествено мъжко подстригване" };
            await dbContext.Services.AddAsync(service);
            await dbContext.SaveChangesAsync();

            var date = DateTime.Today.AddDays(1).AddHours(10);

            await dbContext.Appointments.AddAsync(new Appointment
            {
                ServiceId = 2,
                AppointmentDate = date
            });
            await dbContext.SaveChangesAsync();

            var availableSlots = await appointmentService.GetAvailableSlotsAsync(date.Date, 2);

            Assert.IsFalse(availableSlots.Contains(date));
        }

        [Test]
        public async Task GetAllDatesWithAppointmentsAsync_Should_Return_Unique_Dates()
        {
            var service = new Service { Id = 1, Name = "Мъжко подстригване", DurationInMinutes = 30, Description = "Kaчествено мъжко подстригване" };
            await dbContext.Services.AddAsync(service);

            var date1 = new DateTime(2025, 8, 5, 10, 0, 0);
            var date2 = new DateTime(2025, 8, 6, 11, 0, 0);

            await dbContext.Appointments.AddRangeAsync(
                new Appointment { AppointmentDate = date1, ServiceId = 4 },
                new Appointment { AppointmentDate = date2, ServiceId = 4 }
            );

            await dbContext.SaveChangesAsync();

            var dates = await appointmentService.GetAllDatesWithAppointmentsAsync();

            Assert.That(dates.Count, Is.EqualTo(2));
            Assert.That(dates[0], Is.EqualTo(date1.Date));
            Assert.That(dates[1], Is.EqualTo(date2.Date));
        }
    }
}
