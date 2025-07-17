using Barbershop.Data.Models;
using Barbershop.ViewModels;

namespace Barbershop.Services.Contracts
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceViewModel>> GetAllAsync();
        Task<ServiceViewModel?> GetByIdAsync(int id);
        Task<bool> CreateAsync(ServiceViewModel service);
        Task<bool> UpdateAsync(ServiceViewModel service);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();
    }
}
