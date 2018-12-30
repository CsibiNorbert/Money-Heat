using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace TemperatureV1._0.Models
{
    public class DbMyContext : DbContext
    {
        public DbSet<Customer> customer { get; set; }
        
    }
}