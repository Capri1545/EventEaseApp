using System.ComponentModel.DataAnnotations;

namespace EventEaseApp.Models;

public class Event
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Event name is required.")]
    public string? EventName { get; set; }

    [Required(ErrorMessage = "Date is required.")]
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Required(ErrorMessage = "Location is required.")]
    public string? Location { get; set; }
}
