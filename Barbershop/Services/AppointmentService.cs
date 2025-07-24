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
    }
}
