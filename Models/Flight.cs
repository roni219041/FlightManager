using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Manager.Models
{
    public class Flight
    {
        public Flight()
        {
            DepartureTime = DateTime.Today;
            LandingTime = DateTime.Today;
        }
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string StartLocation { get; set; }
        [Required]
        public string EndLocation { get; set; }
        [Required]
        public DateTime DepartureTime { get; set; }
        [Required]
        public DateTime LandingTime { get; set; }
        [Required]
        public string PlaneId { get; set; }
        [Required]
        public string PlaneModel { get; set; }
        [Required]
        public string PilotName { get; set; }
        [Required]
        public int EconomyClassSeats { get; set; }
        [Required]
        public int BusinessClassSeats { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}
