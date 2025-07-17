using Barbershop.Data;
using Barbershop.Data.Models;
using Barbershop.Services.Contracts;
using Barbershop.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Barbershop.Services
{
    public class ServiceService : IServiceService
    {
        private readonly BarbershopDbContext dbContext;

        public ServiceService(BarbershopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<ServiceViewModel>> GetAllAsync()
        {
            return await dbContext.Services
                .Include(s => s.Category)
                .Select(s => new ServiceViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Price = s.Price,
                    Duration = s.DurationInMinutes,
                    CategoryName = s.Category.Name
                })
                .ToListAsync();
        }

        public async Task<ServiceViewModel?> GetByIdAsync(int id)
        {
            return await dbContext.Services
                .Where(s => s.Id == id)
                .Select(s => new ServiceViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Price = s.Price,
                    Duration = s.DurationInMinutes,
                    CategoryId = s.CategoryId,
                    CategoryName = s.Category.Name
                })
                .FirstOrDefaultAsync();
        }

        async Task<bool> IServiceService.CreateAsync(ServiceViewModel service)
        {
            if (service == null)
            {
                return false;
            }

            var categoryExists = await dbContext.Categories.AnyAsync(c => c.Id == service.CategoryId);
            if (!categoryExists)
            {
                return false;
            }

            Service serviceToAdd = new Service() 
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price,
                DurationInMinutes = service.Duration,
                CategoryId = service.CategoryId
            };

            await dbContext.Services.AddAsync(serviceToAdd);
            await dbContext.SaveChangesAsync();

            return true;
        }

        async Task<bool> IServiceService.DeleteAsync(int id)
        {
            var service = await dbContext.Services.FindAsync(id);
            if (service == null)
                return false;

            dbContext.Services.Remove(service);
            await dbContext.SaveChangesAsync();

            return true;
        }

        async Task<bool> IServiceService.UpdateAsync(ServiceViewModel service)
        {
            if (service == null)
                return false;

            var existingService = await dbContext.Services.FindAsync(service.Id);
            if (existingService == null)
                return false;

            var categoryExists = await dbContext.Categories.AnyAsync(c => c.Id == service.CategoryId);
            if (!categoryExists)
                return false;

            existingService.Name = service.Name;
            existingService.Description = service.Description;
            existingService.Price = service.Price;
            existingService.DurationInMinutes = service.Duration;
            existingService.CategoryId = service.CategoryId;

            dbContext.Services.Update(existingService);
            await dbContext.SaveChangesAsync();

            return true;
        }
        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
        {
            return await dbContext.Categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }
    }
}
