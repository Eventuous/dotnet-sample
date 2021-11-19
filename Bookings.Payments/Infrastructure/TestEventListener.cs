using System.Diagnostics.Tracing;

namespace Bookings.Payments.Infrastructure;

public sealed class TestEventListener : EventListener {
    readonly string[]          _prefixes = { "OpenTelemetry", "eventuous" };
    readonly List<EventSource> _eventSources = new();
    protected override void OnEventSourceCreated(EventSource? eventSource) {
        if (eventSource?.Name == null) {
            return;
        }

        if (_prefixes.Any(x => eventSource.Name.StartsWith(x))) {
            Console.WriteLine($"Event source created: {eventSource.Name}");
            _eventSources.Add(eventSource);
            EnableEvents(eventSource, EventLevel.Verbose, EventKeywords.All);
        }

        base.OnEventSourceCreated(eventSource);
    }

#nullable disable
    protected override void OnEventWritten(EventWrittenEventArgs evt) {
        string message;

        if (evt.Message != null && (evt.Payload?.Count ?? 0) > 0) {
            message = string.Format(evt.Message, evt.Payload.ToArray());
        }
        else {
            message = evt.Message;
        }

        Console.WriteLine(
            $"{evt.EventSource.Name} - EventId: [{evt.EventId}], EventName: [{evt.EventName}], Message: [{message}]"
        );
    }
#nullable enable

    public override void Dispose() {
        foreach (var eventSource in _eventSources) {
            DisableEvents(eventSource);
        }

        base.Dispose();
    }
}