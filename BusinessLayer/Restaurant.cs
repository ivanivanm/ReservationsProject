using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Restaurant
    {
        public Restaurant(string name,string address)
        {
            Name = name;
            Address = address;
        }
        public Restaurant()
        {
            
        }
        [Key]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public List<Reservation> Reservations { get; set; }
        [Range(0,double.MaxValue)]
        public double AnnualIncome { get; set; }
    }
}
