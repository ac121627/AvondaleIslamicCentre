using AvondaleIslamicCentre.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AvondaleIslamicCentre.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AICUser class
public class AICUser : IdentityUser
{
    [Required(ErrorMessage = "Please provide a valid First Name.")]
    [PersonalData] // Marks this property as personal data for GDPR purposes
    [RegularExpression(@"^[A-Z][a-z\s]*$", ErrorMessage = "Name must begin with a capital letter and must not include special characters or numbers.")]
    [Column(TypeName = "varchar(100)")] // Specifies database column type
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please provide a valid Last Name.")]
    [PersonalData] // Marks this property as personal data for GDPR purposes
    [RegularExpression(@"^[A-Z][a-z\s]*$", ErrorMessage = "Name must begin with a capital letter and must not include special characters or numbers.")]
    [Column(TypeName = "varchar(100)")] // Specifies database column type
    public string LastName { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Invalid phone number format")]
    public string Phone { get; set; } = string.Empty;

    public ICollection<Booking> Booking { get; set; } = new List<Booking>(); // Navigation property to Booking
}

