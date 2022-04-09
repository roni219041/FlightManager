using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Manager.Models
{
    public class Passenger
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string LastName { get; set; }
        [RegularExpression(@"\d{10}")]
        [Required]
        public string PersonalNumber { get; set; }
        [Required]
        [RegularExpression(@"08[7|8|9]\d{7}")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Nationality { get; set; }
        [Required]
        public bool IsBusinessClass { get; set; }
        [ForeignKey("ReservationId")]
        public Reservation Reservation { get; set; }
        public Guid ReservationId { get; set; }

    }
}
