using System;
using System.ComponentModel.DataAnnotations;

namespace GymProjectApp.Models
{
    // Represents a gym member entity in the system
    public class Member
    {
        // Primary key for the Member table
        [Key]
        public int MemberID { get; set; }

        // Member's full name (required, max 100 characters)
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name must be at most 100 characters.")]
        public string Name { get; set; } = string.Empty;

        // Member's age (must be between 12 and 100)
        [Range(12, 100, ErrorMessage = "Age must be between 12 and 100.")]
        public int Age { get; set; }

        // Type of membership (required, defined by MembershipType enum)
        [Required(ErrorMessage = "Membership type is required.")]
        public MembershipType MembershipType { get; set; }

        // Date when the member joined the gym (defaults to current date)
        [DataType(DataType.Date)]
        public DateTime JoinDate { get; set; } = DateTime.Now;

        // Optional foreign key to a workout plan (nullable)
        public int? PlanID { get; set; }
    }

    // Enum for membership types to ensure consistency and avoid typos
    
}