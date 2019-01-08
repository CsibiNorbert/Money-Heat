using System;
using System.ComponentModel.DataAnnotations;

namespace TemperatureV1._0.Models
{
    public class Payment
    {
        [Key]
        public int idPayment { get; set; }
        
        public int payment { get; set; }
        
        public DateTime paymentDate { get; set; }
        public double avgTemperature { get; set; }
        public int idUser { get; set; }
    }
}