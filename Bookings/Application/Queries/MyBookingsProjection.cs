using Eventuous.Projections.MongoDB;
using MongoDB.Driver;
using static Bookings.Domain.Bookings.BookingEvents;

namespace Bookings.Application.Queries;

public class MyBookingsProjection : MongoProjection<MyBookings> {
    public MyBookingsProjection(IMongoDatabase database) : base(database) {
        On<V1.RoomBooked>(
            evt => evt.GuestId,
            (evt, update) => update.AddToSet(
                x => x.Bookings,
                new MyBookings.Booking(evt.BookingId, evt.CheckInDate, evt.CheckOutDate, evt.BookingPrice)
            )
        );

        On<V1.BookingCancelled>(
            b => b.UpdateOne
                .Filter(
                    (evt, doc)
                        => doc.Bookings.Select(booking => booking.BookingId).Contains(evt.BookingId)
                )
                .Update(
                    (evt, update) =>
                        update.PullFilter(
                            x => x.Bookings,
                            x => x.BookingId == evt.BookingId
                        )
                )
        );
    }
}
