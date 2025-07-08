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
    }
}
