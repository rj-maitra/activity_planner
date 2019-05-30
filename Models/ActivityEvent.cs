using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityPlanner.Models
{
    public class NoPastDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((DateTime)value <= DateTime.Now)
                return new ValidationResult("Your activity must occur in the future.");
            return ValidationResult.Success;
        }
    }

    public class ActivityEvent
    {
        [Key]
        public int ActivityId { get; set; }

        [Required]
        [MinLength(2)]
        [Display(Name="Title")]
        public string Title { get; set; }

        [Required]
        public string Time { get; set; }

        [Required]
        [NoPastDate(ErrorMessage="Date must be a future date")]
        public DateTime Date { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage="Duration must be greater than 0")]
        public int Duration { get; set; }
        
        [Required]
        [Display(Name="Select")]
        public string DurationSelector { get; set; }

        [Required]
        public string Description { get; set; }

        public User Planner { get; set; }
        public int PlannerId { get; set; }
        public List<Join> Participants { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
