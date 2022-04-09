using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Manager.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"\d{10}")]
        [Display(Name = "Personal number")]
        public string PersonalNumber { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }
        //[Required]
        //[RegularExpression(@"08[7|8|9]\d{7}")]
        //[Display(Name = "Phone Number")]
        //public string PhoneNumber { get; set; }
    }
}
