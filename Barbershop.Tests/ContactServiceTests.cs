using Barbershop.Data;
using Barbershop.Data.Models;
using Barbershop.Services;
using Barbershop.ViewModels;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbershop.Tests
{
    [TestFixture]
    public class ContactServiceTests
    {
        private BarbershopDbContext dbContext = null!;
        private ContactService contactService = null!;

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<BarbershopDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            dbContext = new BarbershopDbContext(options);
            contactService = new ContactService(dbContext);
        }

        [Test]
        public async Task SendMessageAsync_Should_Add_Message()
        {
            var model = new ContactMessageInputModel
            {
                Name = "Иван",
                Email = "ivan@example.com",
                Message = new string('x', 50)
            };

            await contactService.SendMessageAsync(model);

            var messages = dbContext.ContactMessages.ToList();

            Assert.That(messages.Count, Is.EqualTo(1));
            Assert.That(messages[0].Email, Is.EqualTo("ivan@example.com"));
        }

        [Test]
        public async Task GetAllAsync_Should_Return_All_Messages()
        {
            dbContext.ContactMessages.Add(new ContactMessage
            {
                Name = "Мария",
                Email = "maria@example.com",
                Message = "Тестово съобщение",
                SentOn = DateTime.UtcNow
            });
            await dbContext.SaveChangesAsync();

            var result = await contactService.GetAllAsync();

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo("Мария"));
        }
    }
}
