namespace Barbershop.ViewModels
{
    public class ServiceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public TimeSpan Duration { get; set; }
        public string CategoryName { get; set; } = null!;

        public string PriceFormatted => $"{Price:C}";
        public string DurationFormatted => $"{(int)Duration.TotalMinutes} мин";
    }
}
