using Eventuous.Projections.MongoDB;
using MongoDB.Driver;
using static Bookings.Domain.Bookings.BookingEvents;

namespace Bookings.Application.Queries;

public class MyBookingsProjection : MongoProjection<MyBookings> {
    public MyBookingsProjection(IMongoDatabase database) : base(database) {
        On<V1.RoomBooked>(
            ctx =>
                UpdateOperationTask(
                    ctx.Message.GuestId,
                    update => update.AddToSet(
                        x => x.Bookings,
                        new MyBookings.Booking(
                            ctx.Message.BookingId,
                            ctx.Message.CheckInDate,
                            ctx.Message.CheckOutDate,
                            ctx.Message.BookingPrice
                        )
                    )
                )
        );
    }
}