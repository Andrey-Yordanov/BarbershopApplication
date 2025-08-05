using Barbershop.Data;
using Barbershop.Data.Models;
using Barbershop.Services;
using Barbershop.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbershop.Tests
{
    public class ServiceServiceTests
    {
        private BarbershopDbContext dbContext = null!;
        private ServiceService serviceService = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<BarbershopDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            dbContext = new BarbershopDbContext(options);
            serviceService = new ServiceService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        [Test]
        public async Task GetAllAsync_Should_Return_All_Services()
        {
            var category = new Category { Id = 1, Name = "Подстригване" };
            await dbContext.Categories.AddAsync(category);

            await dbContext.Services.AddAsync(new Service
            {
                Name = "Мъжко подстригване",
                Description = "20 мин",
                DurationInMinutes = 20,
                Price = 15.00m,
                CategoryId = 1
            });

            await dbContext.SaveChangesAsync();

            var result = await serviceService.GetAllAsync();

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo("Мъжко подстригване"));
        }

        [Test]
        public async Task GetByIdAsync_Should_Return_Correct_Service()
        {
            var category = new Category { Id = 1, Name = "Бръснене" };
            await dbContext.Categories.AddAsync(category);

            var service = new Service
            {
                Id = 5,
                Name = "Класическо бръснене",
                Description = "15 мин",
                DurationInMinutes = 15,
                Price = 10.00m,
                CategoryId = 1
            };

            await dbContext.Services.AddAsync(service);
            await dbContext.SaveChangesAsync();

            var result = await serviceService.GetByIdAsync(5);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("Класическо бръснене"));
        }

        [Test]
        public async Task CreateAsync_Should_Add_Service_If_Category_Exists()
        {
            await dbContext.Categories.AddAsync(new Category { Id = 2, Name = "Маникюр" });
            await dbContext.SaveChangesAsync();

            var model = new ServiceViewModel
            {
                Name = "Маникюр с гел лак",
                Description = "45 мин",
                Price = 25,
                Duration = 45,
                CategoryId = 2
            };

            var result = await serviceService.CreateAsync(model);

            Assert.IsTrue(result);
            Assert.That(await dbContext.Services.CountAsync(), Is.EqualTo(1));
        }

        [Test]
        public async Task CreateAsync_Should_Return_False_If_Category_Not_Exists()
        {
            var model = new ServiceViewModel
            {
                Name = "Несъществуващ",
                Description = "Тест",
                Price = 5,
                Duration = 10,
                CategoryId = 999
            };

            var result = await serviceService.CreateAsync(model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteAsync_Should_Remove_Existing_Service()
        {
            var service = new Service
            {
                Id = 10,
                Name = "Тест",
                Description = "тест описание",
                DurationInMinutes = 20,
                Price = 20,
                CategoryId = 1,
                Category = new Category { Id = 1, Name = "Тест Категория" }
            };

            await dbContext.Services.AddAsync(service);
            await dbContext.SaveChangesAsync();

            var result = await serviceService.DeleteAsync(10);

            Assert.IsTrue(result);
            Assert.That(await dbContext.Services.CountAsync(), Is.EqualTo(0));
        }

        [Test]
        public async Task DeleteAsync_Should_Return_False_If_Not_Found()
        {
            var result = await serviceService.DeleteAsync(999);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task UpdateAsync_Should_Update_Existing_Service()
        {
            await dbContext.Categories.AddAsync(new Category { Id = 1, Name = "Стара Категория" });
            await dbContext.Categories.AddAsync(new Category { Id = 2, Name = "Нова Категория" });

            var service = new Service
            {
                Id = 100,
                Name = "Стара услуга",
                Description = "Описание",
                DurationInMinutes = 30,
                Price = 30,
                CategoryId = 1
            };

            await dbContext.Services.AddAsync(service);
            await dbContext.SaveChangesAsync();

            var updatedModel = new ServiceViewModel
            {
                Id = 100,
                Name = "Обновена услуга",
                Description = "Ново описание",
                Duration = 45,
                Price = 40,
                CategoryId = 2
            };

            var result = await serviceService.UpdateAsync(updatedModel);

            Assert.IsTrue(result);

            var updated = await dbContext.Services.FindAsync(100);
            Assert.That(updated!.Name, Is.EqualTo("Обновена услуга"));
            Assert.That(updated.CategoryId, Is.EqualTo(2));
        }

        [Test]
        public async Task UpdateAsync_Should_Return_False_If_Service_Not_Found()
        {
            var result = await serviceService.UpdateAsync(new ServiceViewModel
            {
                Id = 999,
                Name = "Несъществуващ",
                Description = "не съществува",
                Duration = 30,
                Price = 20,
                CategoryId = 1
            });

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetAllCategoriesAsync_Should_Return_All_Categories()
        {
            await dbContext.Categories.AddRangeAsync(
                new Category { Id = 1, Name = "Категория 1" },
                new Category { Id = 2, Name = "Категория 2" }
            );

            await dbContext.SaveChangesAsync();

            var result = await serviceService.GetAllCategoriesAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
        }
    }
}
