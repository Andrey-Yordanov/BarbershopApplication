using Barbershop.Services.Contracts;
using Barbershop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Barbershop.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService appointmentService;
        private readonly IServiceService serviceService;

        public AppointmentController(
            IAppointmentService appointmentService,
            IServiceService serviceService)
        {
            this.appointmentService = appointmentService;
            this.serviceService = serviceService;
        }

        [HttpGet("Appointment/Book/{serviceId}")]
        public async Task<IActionResult> Book(int serviceId)
        {
            var service = await serviceService.GetByIdAsync(serviceId);
            if (service == null)
            {
                return NotFound();
            }

            var model = new AppointmentCreateModel
            {
                ServiceId = service.Id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Book(AppointmentCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                var services = await serviceService.GetAllAsync();
                ViewBag.Services = new SelectList(services, "Id", "Name", model.ServiceId);
                return View(model);
            }

            if (!TimeSpan.TryParse(model.Time, out var time))
            {
                ModelState.AddModelError("Time", "Invalid time selected.");
                var services = await serviceService.GetAllAsync();
                ViewBag.Services = new SelectList(services, "Id", "Name", model.ServiceId);
                return View(model);
            }

            model.AppointmentDate = model.AppointmentDate.Date.Add(time);

            await appointmentService.AddAsync(model);
            return RedirectToAction("MyAppointments");
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableSlots(DateTime date, int serviceId)
        {
            var slots = await appointmentService.GetAvailableSlotsAsync(date, serviceId);
            return Json(slots.Select(x => x.ToString("HH:mm")));
        }

        public async Task<IActionResult> MyAppointments()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var appointments = await appointmentService.GetAllByUserAsync(userId);
            return View(appointments);
        }
    }
}
