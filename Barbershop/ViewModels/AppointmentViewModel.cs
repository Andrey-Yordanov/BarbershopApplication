namespace Barbershop.ViewModels
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }
        public string ServiceName { get; set; } = null!;
        public DateTime AppointmentDate { get; set; }
        public string Username { get; set; } = null!;

        public string DateFormatted => AppointmentDate.ToString("dd.MM.yyyy HH:mm");
    }
}
