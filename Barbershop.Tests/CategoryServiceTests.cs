using Barbershop.Data;
using Barbershop.Data.Models;
using Barbershop.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbershop.Tests
{
    public class CategoryServiceTests
    {
        private BarbershopDbContext dbContext = null!;
        private CategoryService categoryService = null!;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<BarbershopDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            dbContext = new BarbershopDbContext(options);

            dbContext.Categories.AddRange(
                new Category { Id = 1, Name = "Коса" },
                new Category { Id = 2, Name = "Брада" }
            );

            await dbContext.SaveChangesAsync();

            categoryService = new CategoryService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllCategories()
        {
            var result = await categoryService.GetAllAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            var names = result.Select(c => c.Name).ToList();
            CollectionAssert.Contains(names, "Коса");
            CollectionAssert.Contains(names, "Брада");
        }

        [Test]
        public async Task GetByIdAsync_WithValidId_ShouldReturnCategory()
        {
            var result = await categoryService.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result!.Id);
            Assert.AreEqual("Коса", result.Name);
        }

        [Test]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            var result = await categoryService.GetByIdAsync(999);

            Assert.IsNull(result);
        }
    }
}
