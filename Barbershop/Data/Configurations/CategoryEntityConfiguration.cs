using Barbershop.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
        }
    }
}
