using Kfz.Database;
using Microsoft.EntityFrameworkCore;

namespace Kfz
{
    public class KfzService: IKfzService
    {
        private readonly KfzDbContext kfzDbContext;

        public KfzService(KfzDbContext kfzDbContext)
        {
            this.kfzDbContext = kfzDbContext;
        }

        public IList<Manufacturer> GetManufacturers()
        {
            return this.kfzDbContext.Manufacturers.Select(x => x).OrderBy(x => x.BasePrice).ToList();
        }

        public IList<Fuel> GetFuels()
        {
            return this.kfzDbContext.Fuels.Select(x => x).OrderBy(x => x.EcoFriendlinessRating).ToList();
        }

        public IList<Motor> GetMotorsForFuel(int fuelId)
        {
            return this.kfzDbContext.Motors.Where(x => x.FuelId == fuelId).ToList();
        }

        public IList<Option> GetOptions()
        {
            return this.kfzDbContext.Options.Select(x => x).OrderBy(x => x.Price).ToList();
        }

        public Order? GetOrderByNumber(int orderNumber)
        {
            return this.kfzDbContext.Orders.Where(x => x.Id == orderNumber)
                .Include("Manufacturer")
                .Include("Motor.Fuel")
                .Include("Motor")
                .Include("Options").FirstOrDefault();
        }

        public IList<Order> GetOrdersByLastnameBirthdate(string lastname, DateOnly birthdate)
        {
            return this.kfzDbContext.Orders
                .Where(x => x.LastName == lastname && x.BirthDate.Equals(birthdate))
                .Include("Manufacturer")
                .Include("Motor.Fuel")
                .Include("Motor")
                .Include("Options")
                .ToList();
        }

        public Order CreateOrder(Order order)
        {
            Order ret = this.kfzDbContext.Orders.Add(order).Entity;
            this.kfzDbContext.SaveChanges();
            return ret;
        }

        public Order UpdateOrder(Order order)
        {
            Order ret = this.kfzDbContext.Orders.Update(order).Entity;
            this.kfzDbContext.SaveChanges();
            return ret;
        }

        public void DeleteOrder(Order order)
        {
            this.kfzDbContext.Orders.Remove(order);
            this.kfzDbContext.SaveChanges();
        }
    }
}
