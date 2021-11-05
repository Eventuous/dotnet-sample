using Eventuous.Projections.MongoDB.Tools;

namespace Bookings.Application.Bookings;

public record MyBookings : ProjectedDocument {
    public MyBookings(string id) : base(id) { }

    public List<Booking> Bookings { get; init; } = new();

    public record Booking(string BookingId, DateTimeOffset CheckInDate, DateTimeOffset CheckOutDate, float Price);
}