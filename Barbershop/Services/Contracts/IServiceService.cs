using Barbershop.Data.Models;
using Barbershop.ViewModels;

namespace Barbershop.Services.Contracts
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceViewModel>> GetAllAsync();
        Task<Service?> GetByIdAsync(int id);
    }
}
