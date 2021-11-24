using Eventuous.Projections.MongoDB;
using Eventuous.Subscriptions.Context;
using MongoDB.Driver;
using static Bookings.Domain.Bookings.BookingEvents;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Bookings.Application.Queries;

public class BookingStateProjection : MongoProjection<BookingDocument> {
    public BookingStateProjection(IMongoDatabase database) : base(database) {
        On<V1.RoomBooked>(HandleRoomBooked);

        On<V1.PaymentRecorded>(
            ctx
                => UpdateOperationTask(
                    ctx.Message.BookingId,
                    update => update.Set(x => x.Outstanding, ctx.Message.Outstanding)
                )
        );

        On<V1.BookingFullyPaid>(
            ctx
                => UpdateOperationTask(ctx.Message.BookingId, update => update.Set(x => x.Paid, true))
        );
    }

    ValueTask<Operation<BookingDocument>> HandleRoomBooked(MessageConsumeContext<V1.RoomBooked> ctx)
        => UpdateOperationTask(
            ctx.Message.BookingId,
            update => update.SetOnInsert(x => x.Id, ctx.Message.BookingId)
                .Set(x => x.GuestId, ctx.Message.GuestId)
                .Set(x => x.RoomId, ctx.Message.RoomId)
                .Set(x => x.CheckInDate, ctx.Message.CheckInDate)
                .Set(x => x.CheckOutDate, ctx.Message.CheckOutDate)
                .Set(x => x.BookingPrice, ctx.Message.BookingPrice)
                .Set(x => x.Outstanding, ctx.Message.OutstandingAmount)
        );
}