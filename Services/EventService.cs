using EventEaseApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace EventEaseApp.Services;

public class EventService
{
    private readonly List<Event> _events = [];

    public event Action? OnChange;

    public Task<List<Event>> GetEventsAsync()
    {
        return Task.FromResult(_events.OrderBy(e => e.Date).ToList());
    }

    public Task AddEventAsync(Event newEvent)
    {
        if (newEvent == null)
            throw new ArgumentNullException(nameof(newEvent));

        _events.Add(newEvent);
        NotifyStateChanged();
        return Task.CompletedTask;
    }

     public Task<Event?> GetEventByIdAsync(Guid id)
    {
        return Task.FromResult(_events.FirstOrDefault(e => e.Id == id));
    }

    public Task UpdateEventAsync(Event updatedEvent)
    {
        var existingEvent = _events.FirstOrDefault(e => e.Id == updatedEvent.Id);
        if (existingEvent != null)
        {
            existingEvent.EventName = updatedEvent.EventName;
            existingEvent.Date = updatedEvent.Date;
            existingEvent.Location = updatedEvent.Location;
            NotifyStateChanged();
        }
        return Task.CompletedTask;
    }

    public Task DeleteEventAsync(Guid id)
    {
        var eventToRemove = _events.FirstOrDefault(e => e.Id == id);
        if (eventToRemove != null)
        {
            _events.Remove(eventToRemove);
            NotifyStateChanged();
        }
        return Task.CompletedTask;
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}

