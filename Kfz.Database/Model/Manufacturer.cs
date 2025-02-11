namespace Kfz.Database
{
    public class Manufacturer
    {
        public int Id { get; set; }

        public required string DisplayName { get; set; }

        public int BasePrice { get; set; }

        public ICollection<Order> Orders { get; } = [];
    }
}
