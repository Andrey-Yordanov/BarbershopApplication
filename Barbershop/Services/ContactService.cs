using Barbershop.Data;
using Barbershop.Services.Contracts;
using Barbershop.ViewModels;
using Microsoft.EntityFrameworkCore;
using Barbershop.Data.Models;

namespace Barbershop.Services
{
    public class ContactService : IContactService
    {
        private readonly BarbershopDbContext dbContext;

        public ContactService(BarbershopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<ContactMessageViewModel>> GetAllAsync()
        {
            return await dbContext.ContactMessages
            .Select(c => new ContactMessageViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Message = c.Message
            })
            .ToListAsync();
        }

        public async Task SendMessageAsync(ContactMessageInputModel model)
        {
            var message = new ContactMessage
            {
                Name = model.Name,
                Email = model.Email,
                Message = model.Message,
                SentOn = DateTime.UtcNow
            };

            await dbContext.ContactMessages.AddAsync(message);
            await dbContext.SaveChangesAsync();
        }
    }
}
