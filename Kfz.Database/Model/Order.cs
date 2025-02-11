namespace Kfz.Database
{
    public class Order
    {
        public int Id { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public DateOnly BirthDate { get; set; }

        public int ManufacturerId { get; set; }
        public Manufacturer? Manufacturer { get; set; }

        public int MotorId { get; set; }
        public Motor? Motor { get; set; }

        public required string Color { get; set; }

        public ICollection<Option> Options { get; } = [];
    }
}
