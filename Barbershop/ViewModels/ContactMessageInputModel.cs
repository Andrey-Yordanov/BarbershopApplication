using System.ComponentModel.DataAnnotations;
using static Barbershop.Common.EntityValidationConstants.ContactMessage;

namespace Barbershop.ViewModels
{
    public class ContactMessageInputModel
    {
        [Required]
        [StringLength(ContactNameMaxLength, MinimumLength = ContactNameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(ContactEmailMaxLength, MinimumLength = ContactEmailMinLength)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(ContactMessageMaxLength, MinimumLength = ContactMessageMinLength)]
        public string Message { get; set; } = null!;
    }
}
