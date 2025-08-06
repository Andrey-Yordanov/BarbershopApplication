using Barbershop.Controllers;
using Barbershop.Services.Contracts;
using Barbershop.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbershop.Tests
{
    [TestFixture]
    public class ContactControllerTests
    {
        private Mock<IContactService> mockContactService;
        private ContactController controller;

        [SetUp]
        public void SetUp()
        {
            mockContactService = new Mock<IContactService>();
            controller = new ContactController(mockContactService.Object);

            controller.TempData = new TempDataDictionary(
                new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        }

        [TearDown]
        public void TearDown()
        {
            controller?.Dispose();
        }

        [Test]
        public void Send_Get_ReturnsView()
        {
            var result = controller.Send() as ViewResult;

            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Send_Post_ValidModel_RedirectsWithSuccessMessage()
        {
            var model = new ContactMessageInputModel
            {
                Name = "Test",
                Email = "test@example.com",
                Message = "Message"
            };

            var result = await controller.Send(model) as RedirectToActionResult;

            mockContactService.Verify(c => c.SendMessageAsync(model), Times.Once);
            Assert.IsNotNull(result);
            Assert.That(result.ActionName, Is.EqualTo("Send"));
            Assert.That(controller.TempData["SuccessMessage"]?.ToString(), Is.EqualTo("Съобщението беше изпратено успешно!"));
        }

        [Test]
        public async Task Send_Post_InvalidModel_ReturnsViewWithModel()
        {
            var model = new ContactMessageInputModel();
            controller.ModelState.AddModelError("Email", "Required");

            var result = await controller.Send(model) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.That(result.Model, Is.EqualTo(model));
            mockContactService.Verify(c => c.SendMessageAsync(It.IsAny<ContactMessageInputModel>()), Times.Never);
        }
    }
}
