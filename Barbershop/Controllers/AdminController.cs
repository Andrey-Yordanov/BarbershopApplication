using Barbershop.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barbershop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAppointmentService appointmentService;

        public AdminController(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> AllAppointments(string? date)
        {
            Console.WriteLine($"Received date parameter: {date}");
            if (string.IsNullOrEmpty(date))
            {
                var uniqueDates = await appointmentService.GetAllDatesWithAppointmentsAsync();
                return View("AppointmentDates", uniqueDates);
            }

            if (!DateTime.TryParse(date, out DateTime parsedDate))
            {
                return RedirectToAction("AllAppointments");
            }

            var appointments = await appointmentService.GetByDateAsync(parsedDate);
            ViewBag.SelectedDate = parsedDate.ToString("yyyy-MM-dd");
            return View("AppointmentsByDate", appointments);
        }
    }
}
