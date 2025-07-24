using Barbershop.Data.Models;
using Barbershop.ViewModels;

namespace Barbershop.Services.Contracts
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentViewModel>> GetAllByUserAsync(string userId);
        Task AddAsync(AppointmentCreateModel model);
        Task<IEnumerable<DateTime>> GetAvailableSlotsAsync(DateTime date, int serviceId);
    }
}
