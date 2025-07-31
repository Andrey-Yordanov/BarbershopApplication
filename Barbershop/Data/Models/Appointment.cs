using Microsoft.AspNetCore.Identity;

namespace Barbershop.Data.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;
    }
}
