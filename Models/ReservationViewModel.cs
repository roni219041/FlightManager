using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Manager.Models
{
    public class ReservationViewModel
    {
        public Guid ReservationId { get; set; }
        public string FlightStartLocation { get; set; }
        public string FlightEndLocation { get; set; }
        public DateTime FlightDepartureTime { get; set; }
        public DateTime CreatedOn { get; set; }
        public int PassengersCount { get; set; }
        public bool IsConfirmed { get; set; }
        public string Email { get; set; }

        public ReservationViewModel()
        {
            CreatedOn = DateTime.Now;
        }

    }
}
