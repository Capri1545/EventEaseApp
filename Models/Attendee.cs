using System.ComponentModel.DataAnnotations;

namespace EventEaseApp.Models;

public class Attendee
{
    [Required]
    [StringLength(100, ErrorMessage = "Name is too long.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; } = string.Empty;
}