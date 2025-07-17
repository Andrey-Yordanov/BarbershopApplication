namespace Barbershop.ViewModels
{
    public class ServiceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string? CategoryName { get; set; }
        public int CategoryId { get; set; }

        public string PriceFormatted => $"{Price:C}";
        public string DurationFormatted => $"{Duration} мин";
    }
}
