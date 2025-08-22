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
    [MinLength(2)]
    [MaxLength(50)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please provide a valid Last Name.")]
    [PersonalData] // Marks this property as personal data for GDPR purposes
    [RegularExpression(@"^[A-Z][a-z\s]*$", ErrorMessage = "Name must begin with a capital letter and must not include special characters or numbers.")]
    [Column(TypeName = "varchar(100)")] // Specifies database column type
    [MinLength(2)]
    [MaxLength(50)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required, Display(Name ="Phone Number")]
    [DataType(DataType.PhoneNumber), MaxLength(17)]
    [RegularExpression(@"^\+((64 (\b(2[0-6])\b)-\d{3,4}-\d{4,5})|(91 \d{5}-\d{5}))$",
        ErrorMessage = "Phone Number is not valid.")]
    public string Phone { get; set; } = string.Empty;

    public ICollection<Booking> Booking { get; set; } = new List<Booking>(); // Navigation property to Booking

    // Navigation property to donations
    public ICollection<Donation> Donations { get; set; } = new List<Donation>();
}