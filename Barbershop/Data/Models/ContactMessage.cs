namespace Barbershop.Data.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Message { get; set; } = null!;

        public DateTime SentOn { get; set; } = DateTime.UtcNow;
    }
}
