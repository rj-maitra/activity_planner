using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityPlanner.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [Display(Name="First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name="Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "Password must contain at least 1 number, 1 letter, 1 special character, and be at least 8 characters long")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Display(Name="Confirm Password")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm { get; set; }
        
        //public List<ActivityEvent> Activities {get;set;}
        public List<Join> Joining  { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}