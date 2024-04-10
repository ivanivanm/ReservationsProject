using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Reservation
    {
        [Key]
        [Range(0,int.MaxValue)]
        public int Id { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int RoomNumber { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Days { get; set; }
        [Required]
        public DateTime DateOfArrival { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
        public Restaurant Restaurant { get; set; }

        [ForeignKey(nameof(Restaurant))]
        public string ReastaurantName { get; set; }

        public Client Client;
        public Reservation()
        {
            
        }
        public Reservation( int roomNumber, int days, DateTime dateOfArrival, double price)
        {
            RoomNumber = roomNumber;
            Days = days;
            DateOfArrival = dateOfArrival;
            Price = price;
        }

        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }
    }
}
