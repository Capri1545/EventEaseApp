using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventEaseApp.Models;

public class Event
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Event name is required.")]
    public string? EventName { get; set; }

    [Required(ErrorMessage = "Date is required.")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Location is required.")]
    public string Location { get; set; } = string.Empty;

    // Add a list to store attendees
    public List<Attendee> Attendees { get; set; } = new List<Attendee>();
}
