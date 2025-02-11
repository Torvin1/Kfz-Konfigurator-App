namespace Kfz.Database
{
    public class Motor
    {
        public int Id { get; set; }

        public required string DisplayName { get; set; }

        public int Price { get; set; } 

        public int FuelId { get; set; }
        public Fuel? Fuel { get; set; }

        public ICollection<Order> Orders { get; } = [];

    }
}
