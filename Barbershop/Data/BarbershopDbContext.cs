using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Barbershop.Data.Models;
using System.Reflection;

namespace Barbershop.Data
{
    public class BarbershopDbContext : IdentityDbContext
    {
        public BarbershopDbContext(DbContextOptions<BarbershopDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Service> Services { get; set; } = null!;

        public virtual DbSet<Category> Categories { get; set; } = null!;

        public virtual DbSet<Appointment> Appointments { get; set; } = null!;

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;

        public virtual DbSet<ContactMessage> ContactMessages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
