using System.ComponentModel.DataAnnotations.Schema;

namespace Barbershop.ViewModels
{
    public class AppointmentCreateModel
    {
        public string UserId { get; set; } = null!;
        public int ServiceId { get; set; }
        public DateTime AppointmentDate { get; set; }

        [NotMapped]
        public string Time { get; set; } = null!;
    }
}
