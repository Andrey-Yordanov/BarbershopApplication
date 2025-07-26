using Barbershop.Data;
using Barbershop.Data.Models;
using Barbershop.Services.Contracts;
using Barbershop.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Barbershop.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly BarbershopDbContext dbContext;

        public AppointmentService(BarbershopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddAsync(AppointmentCreateModel model)
        {
            var appointment = new Appointment
            {
                UserId = model.UserId,
                ServiceId = model.ServiceId,
                AppointmentDate = model.AppointmentDate,
            };

            await dbContext.Appointments.AddAsync(appointment);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AppointmentViewModel>> GetAllByUserAsync(string userId)
        {
            return await dbContext.Appointments
            .Where(a => a.UserId == userId)
            .Select(a => new AppointmentViewModel
            {
                Id = a.Id,
                ServiceName = a.Service.Name,
                AppointmentDate = a.AppointmentDate,
            })
            .ToListAsync();
        }
        public async Task<IEnumerable<DateTime>> GetAvailableSlotsAsync(DateTime date, int serviceId)
        {
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                return Enumerable.Empty<DateTime>();
            }

            var service = await dbContext.Services.FindAsync(serviceId);
            if (service == null)
            {
                throw new ArgumentException("Invalid service ID.");
            }

            var slotDuration = TimeSpan.FromMinutes(service.DurationInMinutes);
            var startHour = 9;
            var endHour = 20;

            var slots = new List<DateTime>();
            var currentTime = date.Date.AddHours(startHour);
            var dayEnd = date.Date.AddHours(endHour);

            while (currentTime + slotDuration <= dayEnd)
            {
                slots.Add(currentTime);
                currentTime = currentTime.AddMinutes(15);
            }

            var takenSlots = await dbContext.Appointments
                .Where(a => a.AppointmentDate.Date == date.Date)
                .Include(a => a.Service)
                .Select(a => new
                {
                    Start = a.AppointmentDate,
                    End = a.AppointmentDate.AddMinutes(a.Service.DurationInMinutes)
                })
                .ToListAsync();

            var available = new List<DateTime>();

            foreach (var slot in slots)
            {
                var slotEnd = slot + slotDuration;

                var overlaps = takenSlots.Any(t =>
                    (slot < t.End) && (slotEnd > t.Start)
                );

                if (!overlaps)
                {
                    available.Add(slot);
                }
            }

            return available;
        }
        public async Task<IEnumerable<AppointmentViewModel>> GetAllAsync()
        {
            return await dbContext.Appointments
                .Include(a => a.Service)
                .Select(a => new AppointmentViewModel
                {
                    Id = a.Id,
                    ServiceName = a.Service.Name,
                    AppointmentDate = a.AppointmentDate
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<AppointmentViewModel>> GetByDateAsync(DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            var result = await dbContext.Appointments
                .Where(a => a.AppointmentDate >= startOfDay && a.AppointmentDate < endOfDay)
                .Include(a => a.Service)
                .Include(a => a.User)
                .Select(a => new AppointmentViewModel
                {
                    Id = a.Id,
                    ServiceName = a.Service.Name,
                    AppointmentDate = a.AppointmentDate,
                    Username = a.User.UserName
                })
                .ToListAsync();

            Console.WriteLine($"Appointments found for {date.ToShortDateString()}: {result.Count}");
            return result;
        }
        public async Task<List<DateTime>> GetAllDatesWithAppointmentsAsync()
        {
            return await dbContext.Appointments
                .Select(a => a.AppointmentDate.Date)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();
        }
    }
}
