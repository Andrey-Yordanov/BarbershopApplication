using Barbershop.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Barbershop.Common.EntityValidationConstants.Service;

namespace Barbershop.Data.Configurations
{
    public class ServiceEntityConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> entity)
        {
            entity
                .HasKey(s => s.Id);

            entity
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(ServiceNameMaxLength);

            entity
                .Property(s => s.Description)
                .IsRequired()
                .HasMaxLength(ServiceDescriptionMaxLength);

            entity
                .Property(s => s.Price)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            entity
                .Property(s => s.DurationInMinutes)
                .IsRequired();

            entity
                .HasOne(s => s.Category)
                .WithMany(c => c.Services)
                .HasForeignKey(s => s.CategoryId);

            entity
                .HasData(
            new Service { Id = 1, Name = "Мъжко подстригване", CategoryId = 1, Price = 15, DurationInMinutes = 30, Description = "Бързо и качествено подстригване" },
            new Service { Id = 2, Name = "Женско боядисване", CategoryId = 2, Price = 50, DurationInMinutes = 90, Description = "Професионално боядисване с висококачествени бои" }
        );
        }
    }
}
