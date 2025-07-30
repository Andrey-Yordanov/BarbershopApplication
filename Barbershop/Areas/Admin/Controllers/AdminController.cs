using Barbershop.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barbershop.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAppointmentService appointmentService;
        private readonly IContactService contactService;

        public AdminController(IAppointmentService appointmentService, IContactService contactService)
        {
            this.appointmentService = appointmentService;
            this.contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> AllAppointments(DateTime? date)
        {
            if (date == null)
            {
                var uniqueDates = await appointmentService.GetAllDatesWithAppointmentsAsync();
                return View("AppointmentDates", uniqueDates);
            }

            var normalizedDate = DateTime.SpecifyKind(date.Value.Date, DateTimeKind.Unspecified);

            var appointments = await appointmentService.GetByDateAsync(normalizedDate);
            ViewBag.SelectedDate = normalizedDate.ToString("yyyy-MM-dd");

            return View("AppointmentsByDate", appointments);
        }

        [HttpGet]
        public async Task<IActionResult> AllMessages()
        {
            var messages = await contactService.GetAllAsync();
            return View("AllMessages", messages);
        }
    }
}
