using Barbershop.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Barbershop.Common.EntityValidationConstants.ApplicationUser;

namespace Barbershop.Data.Configurations
{
    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> entity)
        {
            entity
                .Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(UserFirstNameMaxLength);

            entity
                .Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(UserLastNameMaxLength);
        }
    }
}
