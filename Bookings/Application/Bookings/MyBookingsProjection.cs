using Eventuous.Projections.MongoDB;
using MongoDB.Driver;
using static Bookings.Domain.Bookings.BookingEvents;

namespace Bookings.Application.Bookings; 

public class MyBookingsProjection : MongoProjection<MyBookings> {
    public MyBookingsProjection(IMongoDatabase database) : base(database) { }

    protected override ValueTask<Operation<MyBookings>> GetUpdate(object evt, long? position) {
        return evt switch {
            V1.RoomBooked e => UpdateOperationTask(
                e.GuestId,
                update => update.AddToSet(
                    x => x.Bookings,
                    new MyBookings.Booking(e.BookingId, e.CheckInDate, e.CheckOutDate, e.BookingPrice)
                )
            ),
            _ => NoOp
        };
    }
}