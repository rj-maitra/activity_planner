using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityPlanner.Models
{
    public class Join
    {
        [Key]
        public int JoinId { get; set; }
        public int UserId { get; set; }
        public int ActivityId { get; set; }
        public User Participant { get; set; }
        //user that is participating
        public ActivityEvent Activity { get; set; }
    }
}