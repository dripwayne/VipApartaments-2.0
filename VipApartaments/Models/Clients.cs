using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VipApartaments.Models
{
    public partial class Clients
    {
        public Clients()
        {
            Booking = new HashSet<Booking>();
        }

        public int Id { get; set; }
        [Required]  
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string password { get; set; }


        public virtual ICollection<Booking> Booking { get; set; }
    }
}
