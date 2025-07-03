namespace Barbershop.Data.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<Service> Services { get; set; } =
            new HashSet<Service>();
    }
}
