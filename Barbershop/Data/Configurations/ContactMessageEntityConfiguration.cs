using Barbershop.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Barbershop.Common.EntityValidationConstants.ContactMessage;

namespace Barbershop.Data.Configurations
{
    public class ContactMessageEntityConfiguration : IEntityTypeConfiguration<ContactMessage>
    {
        public void Configure(EntityTypeBuilder<ContactMessage> entity)
        {
            entity
                .HasKey(cm => cm.Id);

            entity
                .Property(cm => cm.Name)
                .IsRequired()
                .HasMaxLength(ContactNameMaxLength);

            entity
                .Property(cm => cm.Email)
                .IsRequired()
                .HasMaxLength(ContactEmailMaxLength);

            entity
                .Property(cm => cm.Message)
                .IsRequired()
                .HasMaxLength(ContactMessageMaxLength);

            entity
                .Property(cm => cm.SentOn)
                .IsRequired();
        }
    }
}
