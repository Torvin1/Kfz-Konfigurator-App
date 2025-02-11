namespace Kfz.Database
{
    public class Option
    {
        public int Id { get; set; }

        public required string DisplayName { get; set; }

        public int Price { get; set; }

        public ICollection<Order> Orders { get; } = [];
    }
}
