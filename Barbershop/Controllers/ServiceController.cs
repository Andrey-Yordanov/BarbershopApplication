using Barbershop.Data.Models;
using Barbershop.Services;
using Barbershop.Services.Contracts;
using Barbershop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Barbershop.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IServiceService _serviceService;
        private readonly ICategoryService _categoryService;


        public ServiceController(IServiceService serviceService, ICategoryService categoryService)
        {
            _serviceService = serviceService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> All()
        {
            var services = await _serviceService.GetAllAsync();
            return View(services);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _serviceService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View(new ServiceViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceViewModel service)
        {
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    var errors = entry.Value?.Errors;
                    foreach (var error in errors!)
                    {
                        Console.WriteLine($"ModelState Error - Key: {key}, Error: {error.ErrorMessage}");
                    }
                }

                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");

                return View(service);
            }

            var success = await _serviceService.CreateAsync(service);
            if (!success)
            {
                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");

                ModelState.AddModelError("", "Неуспешно създаване.");
                return View(service);
            }

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var service = await _serviceService.GetByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", service.CategoryId);

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceViewModel service)
        {
            if (id != service.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(service);

            var success = await _serviceService.UpdateAsync(service);
            if (!success)
            {
                ModelState.AddModelError("", "Failed to update the service. Check your inputs.");
                return View(service);
            }

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _serviceService.GetByIdAsync(id);
            if (service == null)
                return NotFound();

            return View(service);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _serviceService.DeleteAsync(id);
            if (!success)
            {
                ModelState.AddModelError("", "Failed to delete the service.");
                var service = await _serviceService.GetByIdAsync(id);
                return View(service);
            }

            return RedirectToAction(nameof(All));
        }
    }
}
