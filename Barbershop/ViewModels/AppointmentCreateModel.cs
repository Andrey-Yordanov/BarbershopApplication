namespace Barbershop.ViewModels
{
    public class AppointmentCreateModel
    {
        public string UserId { get; set; } = null!;
        public int ServiceId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
