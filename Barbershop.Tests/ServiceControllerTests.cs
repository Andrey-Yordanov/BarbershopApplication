using Barbershop.Controllers;
using Barbershop.Services.Contracts;
using Barbershop.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class ServiceControllerTests
    {
        private Mock<IServiceService> mockServiceService;
        private Mock<ICategoryService> mockCategoryService;
        private ServiceController controller;

        [SetUp]
        public void SetUp()
        {
            mockServiceService = new Mock<IServiceService>();
            mockCategoryService = new Mock<ICategoryService>();

            controller = new ServiceController(mockServiceService.Object, mockCategoryService.Object);

            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        }

        [TearDown]
        public void TearDown()
        {
            controller?.Dispose();
        }

        [Test]
        public async Task All_ReturnsViewWithServices()
        {
            var services = new List<ServiceViewModel>
                {
                      new ServiceViewModel { Id = 1, Name = "Test", Price = 10 }
                };

            mockServiceService.Setup(s => s.GetAllAsync()).ReturnsAsync(services);

            var result = await controller.All() as ViewResult;

            Assert.IsNotNull(result);
            Assert.That(result.Model, Is.EqualTo(services));
        }

        [Test]
        public async Task Create_Get_ReturnsViewWithCategories()
        {
            var categories = new List<CategoryViewModel>
                 {
                     new CategoryViewModel { Id = 1, Name = "Hair" }
                 };

            mockServiceService.Setup(s => s.GetAllCategoriesAsync()).ReturnsAsync(categories);

            var result = await controller.Create() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ServiceViewModel>(result.Model);
            Assert.IsTrue(controller.ViewBag.Categories is SelectList);
        }

        [Test]
        public async Task Create_Post_ValidModel_RedirectsToAll()
        {
            var model = new ServiceViewModel { Id = 1, Name = "Test", CategoryId = 1 };
            mockServiceService.Setup(s => s.CreateAsync(model)).ReturnsAsync(true);

            var result = await controller.Create(model) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.That(result.ActionName, Is.EqualTo("All"));
        }

        [Test]
        public async Task Create_Post_InvalidModel_ReturnsViewWithModel()
        {
            var model = new ServiceViewModel();
            controller.ModelState.AddModelError("Name", "Required");

            var categories = new List<CategoryViewModel> { new CategoryViewModel { Id = 1, Name = "Hair" } };
            mockCategoryService.Setup(c => c.GetAllAsync()).ReturnsAsync(categories);

            var result = await controller.Create(model) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.That(result.Model, Is.EqualTo(model));
        }

        [Test]
        public async Task Edit_Get_ReturnsViewWithService()
        {
            var model = new ServiceViewModel { Id = 1, Name = "Test", CategoryId = 1 };
            mockServiceService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(model);
            mockCategoryService.Setup(c => c.GetAllAsync()).ReturnsAsync(new List<CategoryViewModel>());

            var result = await controller.Edit(1) as ViewResult;

            Assert.IsNotNull(result);
            Assert.That(result.Model, Is.EqualTo(model));
        }

        [Test]
        public async Task Edit_Get_NotFoundIfServiceNull()
        {
            mockServiceService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((ServiceViewModel)null!);

            var result = await controller.Edit(99);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Edit_Post_Valid_RedirectsToAll()
        {
            var model = new ServiceViewModel { Id = 1, Name = "Test", CategoryId = 1 };
            mockServiceService.Setup(s => s.UpdateAsync(model)).ReturnsAsync(true);

            var result = await controller.Edit(1, model) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.That(result.ActionName, Is.EqualTo("All"));
        }

        [Test]
        public async Task Edit_Post_BadRequestIfIdMismatch()
        {
            var model = new ServiceViewModel { Id = 2 };

            var result = await controller.Edit(1, model);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Delete_Get_ReturnsViewWithService()
        {
            var model = new ServiceViewModel { Id = 1, Name = "To Delete" };
            mockServiceService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(model);

            var result = await controller.Delete(1) as ViewResult;

            Assert.IsNotNull(result);
            Assert.That(result.Model, Is.EqualTo(model));
        }

        [Test]
        public async Task Delete_Get_ReturnsNotFoundIfNull()
        {
            mockServiceService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((ServiceViewModel)null!);

            var result = await controller.Delete(99);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeleteConfirmed_Success_RedirectsToAll()
        {
            mockServiceService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await controller.DeleteConfirmed(1) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.That(result.ActionName, Is.EqualTo("All"));
        }

        [Test]
        public async Task DeleteConfirmed_Fails_ReturnsViewWithModel()
        {
            var model = new ServiceViewModel { Id = 1 };
            mockServiceService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);
            mockServiceService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(model);

            var result = await controller.DeleteConfirmed(1) as ViewResult;

            Assert.IsNotNull(result);
            Assert.That(result.Model, Is.EqualTo(model));
        }
    }
}