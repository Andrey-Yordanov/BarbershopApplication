using Barbershop.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using static Barbershop.Common.EntityValidationConstants.Category;

namespace Barbershop.Data.Configurations
{
    public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            entity
                .HasKey(c => c.Id);

            entity
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(CategoryNameMaxLength);

            entity
                .HasMany(c => c.Services)
                .WithOne(s => s.Category)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasData(
            new Category { Id = 1, Name = "Подстригване" },
            new Category { Id = 2, Name = "Боядисване" },
            new Category { Id = 3, Name = "Сешоар" },
            new Category { Id = 4, Name = "Изправяне" },
            new Category { Id = 5, Name = "Плитки" }
        );
        }
    }
}
