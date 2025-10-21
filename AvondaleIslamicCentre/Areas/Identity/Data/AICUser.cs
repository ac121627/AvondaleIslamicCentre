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
    [RegularExpression(@"^[A-Z][a-zA-Z]*(?:[ '\-][A-Za-z][a-zA-Z]*)*$",
    ErrorMessage = "Name must start with a capital letter and may only contain letters, spaces, hyphens, or apostrophes.")]
    [Column(TypeName = "varchar(100)")] // Specifies database column type
    [MinLength(1)]
    [MaxLength(25)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } 

    [Required(ErrorMessage = "Please provide a valid Last Name.")]
    [PersonalData] // Marks this property as personal data for GDPR purposes
    [RegularExpression(@"^[A-Z][a-zA-Z]*(?:[ '\-][A-Za-z][a-zA-Z]*)*$",
    ErrorMessage = "Name must start with a capital letter and may only contain letters, spaces, hyphens, or apostrophes.")]
    [Column(TypeName = "varchar(100)")] // Specifies database column type
    [MinLength(1)]
    [MaxLength(25)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } 

    [Required, Display(Name ="Phone Number")]
    [DataType(DataType.PhoneNumber), MaxLength(15)]
    [RegularExpression(@"^(\+64\s?\d{1,2}\s?\d{3,4}\s?\d{3,4}|0\d{1,2}\s?\d{3,4}\s?\d{3,4})$",
    ErrorMessage = "Please enter a valid New Zealand phone number (e.g., +64 21 234 5678 or 021 234 5678).")]
    public string Phone { get; set; } 

    public ICollection<Booking> Booking { get; set; } = new List<Booking>(); // Navigation property to Booking

    // Navigation property to donations
    public ICollection<Donation> Donations { get; set; } = new List<Donation>();
}