using System.Data.Entity;

namespace TemperatureV1._0.Models
{
    public class DbMyContext : DbContext
    {
        public DbSet<Customer> customer { get; set; }
    }
}