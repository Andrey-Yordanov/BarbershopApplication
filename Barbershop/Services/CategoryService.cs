using Barbershop.Data;
using Barbershop.Data.Models;
using Barbershop.Services.Contracts;
using Barbershop.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Barbershop.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly BarbershopDbContext dbContext;

        public CategoryService(BarbershopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<CategoryViewModel>> GetAllAsync()
        {
            return await dbContext.Categories
            .Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
            })
            .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        => await dbContext.Categories.FindAsync(id);
    }
}
