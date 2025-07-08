using Barbershop.ViewModels;
using Barbershop.Data.Models;

namespace Barbershop.Services.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewModel>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
    }
}
