using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityPlanner.Models
{
    public class LoginUser
    {
        [Required]
        [EmailAddress]

        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password, ErrorMessage="Password must be at least 2 characters.")]
        public string Password { get; set; }
    }
}