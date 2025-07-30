//using Barbershop.Services.Contracts;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace Barbershop.Controllers
//{
//    [Authorize(Roles = "Admin")]
//    public class _AdminController : Controller
//    {
//        private readonly IAppointmentService appointmentService;
//        private readonly IContactService contactService;

//        public _AdminController(IAppointmentService appointmentService, IContactService contactService)
//        {
//            this.appointmentService = appointmentService;
//            this.contactService = contactService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> AllAppointments(DateTime? date)
//        {
//            if (date == null)
//            {
//                var uniqueDates = await appointmentService.GetAllDatesWithAppointmentsAsync();
//                return View("AppointmentDates", uniqueDates);
//            }

//            var normalizedDate = DateTime.SpecifyKind(date.Value.Date, DateTimeKind.Unspecified);

//            Console.WriteLine($"[DEBUG] Normalized date: {normalizedDate} | Kind: {normalizedDate.Kind}");

//            var appointments = await appointmentService.GetByDateAsync(normalizedDate);
//            ViewBag.SelectedDate = normalizedDate.ToString("yyyy-MM-dd");

//            return View("AppointmentsByDate", appointments);
//        }

//        [HttpGet]
//        public async Task<IActionResult> AllMessages()
//        {
//            var messages = await contactService.GetAllAsync();
//            return View("AllMessages", messages);
//        }
//    }
//}
