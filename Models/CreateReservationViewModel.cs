using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Manager.Models
{
    public class CreateReservationViewModel
    {
        public Guid FlightId { get; set; }
        public List<SelectListItem> Flights { get; set; }
        public List<PassengerViewModel> Passengers { get; set; }
        public string Email { get; set; }
    }

    public class PassengerViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"\d{10}")]
        public string PersonalNumber { get; set; }
        [Required]
        [RegularExpression(@"08[7|8|9]\d{7}")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Nationality { get; set; }
        [Required]
        public bool IsBusinessClass { get; set; }
    }
}
