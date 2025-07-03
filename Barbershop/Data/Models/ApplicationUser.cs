using Microsoft.AspNetCore.Identity;

namespace Barbershop.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public virtual ICollection<Appointment> Appointments { get; set; } =
            new HashSet<Appointment>();
    }
}
