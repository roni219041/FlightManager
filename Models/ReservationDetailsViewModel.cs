using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Manager.Models
{
    public class ReservationDetailsViewModel
    {
        public Guid ReservationId { get; set; }
        public string FlightStartLocation { get; set; }
        public string FlightEndLocation { get; set; }
        public DateTime FlightDepartureTime { get; set; }
        public DateTime FlightArrivalTime { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Email { get; set; }
        public ReservationDetailsViewModel()
        {
            CreatedOn = DateTime.Now;
        }
        public IList<Passenger> Passengers { get; set; }
    }
}
