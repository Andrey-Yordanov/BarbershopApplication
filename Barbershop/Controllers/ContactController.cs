using Barbershop.Services.Contracts;
using Barbershop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Barbershop.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService contactService;

        public ContactController(IContactService contactService)
        {
            this.contactService = contactService;
        }

        [HttpGet]
        public IActionResult Send()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Send(ContactMessageInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await contactService.SendMessageAsync(model);
            TempData["SuccessMessage"] = "Съобщението беше изпратено успешно!";
            return RedirectToAction("Send");
        }
    }
}
