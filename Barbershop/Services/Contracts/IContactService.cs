using Barbershop.Data.Models;
using Barbershop.ViewModels;

namespace Barbershop.Services.Contracts
{
    public interface IContactService
    {
        Task<IEnumerable<ContactMessageViewModel>> GetAllAsync();
        Task SendMessageAsync(ContactMessageInputModel model);
    }
}
