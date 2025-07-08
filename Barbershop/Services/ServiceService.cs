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
                    Price = s.Price,
                    Duration = s.DurationInMinutes,
                    CategoryName = s.Category.Name
                })
                .ToListAsync();
        }

        public async Task<Service?> GetByIdAsync(int id)
            => await dbContext.Services.FindAsync(id);
    }
}
