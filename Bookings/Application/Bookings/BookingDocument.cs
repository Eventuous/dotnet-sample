using Eventuous.Projections.MongoDB.Tools;

namespace Bookings.Application.Bookings;

public record BookingDocument : ProjectedDocument {
    public BookingDocument(string id) : base(id) { }

    public string         GuestId      { get; init; }
    public string         RoomId       { get; init; }
    public DateTimeOffset CheckInDate  { get; init; }
    public DateTimeOffset CheckOutDate { get; init; }
    public float          BookingPrice { get; init; }
    public float          PaidAmount   { get; init; }
    public float          Outstanding  { get; init; }
    public bool           Paid         { get; init; }
}