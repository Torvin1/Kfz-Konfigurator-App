namespace Kfz.Database
{
    public class Fuel
    {
        public int Id { get; set; }

        public required string DisplayName { get; set; }

        public int EcoFriendlinessRating { get; set; } 
    
        public ICollection<Motor> Motors { get; } = [];

        public ICollection<Order> Orders { get; } = [];
    }
}
