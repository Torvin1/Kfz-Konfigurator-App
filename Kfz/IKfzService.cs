using Kfz.Database;

namespace Kfz
{
    public interface IKfzService
    {

        public IList<Manufacturer> GetManufacturers();

        public IList<Fuel> GetFuels();

        public IList<Motor> GetMotorsForFuel(int fuelId);

        public IList<Option> GetOptions();

        public Order? GetOrderByNumber(int orderNumber);

        public IList<Order> GetOrdersByLastnameBirthdate(string lastname, DateOnly birthdate);

        public Order CreateOrder(Order order);

        public Order UpdateOrder(Order order);

        public void DeleteOrder(Order order);
    }
}
