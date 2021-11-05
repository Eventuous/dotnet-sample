using Eventuous.Projections.MongoDB;
using MongoDB.Driver;
using static Bookings.Domain.Bookings.BookingEvents;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Bookings.Application.Bookings;

public class BookingStateProjection : MongoProjection<BookingDocument> {
    public BookingStateProjection(IMongoDatabase database) : base(database) { }

    protected override ValueTask<Operation<BookingDocument>> GetUpdate(object evt, long? position)
        => evt switch {
            V1.RoomBooked e => UpdateOperationTask(
                e.BookingId,
                update => update
                    .SetOnInsert(x => x.Id, e.BookingId)
                    .Set(x => x.GuestId, e.GuestId)
                    .Set(x => x.RoomId, e.RoomId)
                    .Set(x => x.CheckInDate, e.CheckInDate)
                    .Set(x => x.CheckOutDate, e.CheckOutDate)
                    .Set(x => x.BookingPrice, e.BookingPrice)
                    .Set(x => x.Outstanding, e.OutstandingAmount)
            ),
            V1.PaymentRecorded e => UpdateOperationTask(e.BookingId, update => update.Set(x => x.Outstanding, e.Outstanding)),
            V1.BookingFullyPaid e => UpdateOperationTask(e.BookingId, update => update.Set(x => x.Paid, true)),
            _ => NoOp
        };
}