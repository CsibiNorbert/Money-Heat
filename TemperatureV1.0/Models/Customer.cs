using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TemperatureV1._0.Models
{
    
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        //Here we add the validation
        [Required(ErrorMessage ="First Name is required.")]
        public  string FName { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        public  string LName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",ErrorMessage ="Enter a valid email")]
        public  string Email { get; set; }
        public string City { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
       
        [Compare("Password",ErrorMessage ="Please confirm your password ")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public double dailyTemp { get; set; }
        



    }
}