using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Manager.Models
{
    public class Reservation
    {
        public Reservation()
        {
            Passengers = new List<Passenger>();
            CreatedOn = DateTime.Now;
            IsConfirmed = false;
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public Guid FlightId { get; set; }
        [ForeignKey("FlightId")]
        public Flight Flight { get; set; }
        public IList<Passenger> Passengers { get; set; }
        public bool IsConfirmed { get; set; }
        public string Email { get; set; }
    }
}
