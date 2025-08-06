using Barbershop.Controllers;
using Barbershop.Services.Contracts;
using Barbershop.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Barbershop.Tests
{
    [TestFixture]
    public class AppointmentControllerTests
    {
        private Mock<IAppointmentService> mockAppointmentService;
        private Mock<IServiceService> mockServiceService;
        private AppointmentController controller;

        [SetUp]
        public void SetUp()
        {
            mockAppointmentService = new Mock<IAppointmentService>();
            mockServiceService = new Mock<IServiceService>();
            controller = new AppointmentController(mockAppointmentService.Object, mockServiceService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            controller?.Dispose();
        }

        [Test]
        public async Task Book_Get_ReturnsViewWithModel_WhenServiceExists()
        {
            var serviceId = 1;
            mockServiceService.Setup(s => s.GetByIdAsync(serviceId)).ReturnsAsync(new ServiceViewModel { Id = serviceId });

            var result = await controller.Book(serviceId) as ViewResult;

            Assert.IsNotNull(result);
            var model = result.Model as AppointmentCreateModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(serviceId, model.ServiceId);
        }

        [Test]
        public async Task Book_Get_ReturnsNotFound_WhenServiceDoesNotExist()
        {
            var serviceId = 999;
            mockServiceService.Setup(s => s.GetByIdAsync(serviceId)).ReturnsAsync((ServiceViewModel?)null);

            var result = await controller.Book(serviceId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task Book_Post_InvalidModelState_ReturnsViewWithModel()
        {
            controller.ModelState.AddModelError("Test", "TestError");
            var model = new AppointmentCreateModel { ServiceId = 1 };

            mockServiceService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<ServiceViewModel>());

            var result = await controller.Book(model) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreSame(model, result.Model);
        }

        [Test]
        public async Task Book_Post_InvalidTime_ReturnsViewWithError()
        {
            var model = new AppointmentCreateModel
            {
                ServiceId = 1,
                Time = "invalid-time"
            };

            mockServiceService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<ServiceViewModel>());

            var result = await controller.Book(model) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(controller.ModelState.ContainsKey("Time"));
        }

        [Test]
        public async Task Book_Post_ValidModel_RedirectsToMyAppointments()
        {
            var model = new AppointmentCreateModel
            {
                ServiceId = 1,
                AppointmentDate = DateTime.Today,
                Time = "10:00"
            };

            mockAppointmentService.Setup(x => x.AddAsync(It.IsAny<AppointmentCreateModel>())).Returns(Task.CompletedTask);

            var result = await controller.Book(model) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("MyAppointments", result.ActionName);
        }
        [Test]
        public async Task GetAvailableSlots_ReturnsJsonResult_WithFormattedTimes()
        {
            var date = DateTime.Today;
            var serviceId = 1;
            var times = new List<DateTime>
             {
                  date.AddHours(10),
                  date.AddHours(11)
             };

            mockAppointmentService.Setup(s => s.GetAvailableSlotsAsync(date, serviceId))
                .ReturnsAsync(times);

            var result = await controller.GetAvailableSlots(date, serviceId) as JsonResult;

            Assert.IsNotNull(result);
            var timeStrings = result.Value as IEnumerable<string>;
            CollectionAssert.AreEquivalent(new[] { "10:00", "11:00" }, timeStrings);
        }
        [Test]
        public async Task MyAppointments_ReturnsViewWithAppointments_WhenUserIsAuthenticated()
        {
            var userId = "user-123";
            var appointments = new List<AppointmentViewModel>
                {
                     new AppointmentViewModel { Id = 1 }
                };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                         new Claim(ClaimTypes.NameIdentifier, userId)
                }));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            mockAppointmentService.Setup(s => s.GetAllByUserAsync(userId)).ReturnsAsync(appointments);

            var result = await controller.MyAppointments() as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(appointments, result.Model);
        }

        [Test]
        public async Task MyAppointments_RedirectsToLogin_WhenUserIsNotAuthenticated()
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.MyAppointments() as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Login", result.ActionName);
            Assert.AreEqual("Account", result.ControllerName);
        }
    }
}
