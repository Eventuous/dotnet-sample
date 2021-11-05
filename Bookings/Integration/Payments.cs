using Bookings.Domain.Bookings;
using Eventuous;
using Eventuous.Subscriptions;
using Eventuous.Subscriptions.Context;
using static Bookings.Application.Bookings.BookingCommands;
using static Bookings.Integration.IntegrationEvents;
using EventHandler = Eventuous.Subscriptions.EventHandler;

namespace Bookings.Integration;

public class PaymentsIntegrationHandler : EventHandler {
    public static readonly StreamName Stream = new("PaymentsIntegration");

    readonly IApplicationService<BookingState, BookingId> _applicationService;

    public PaymentsIntegrationHandler(IApplicationService<BookingState, BookingId> applicationService) {
        _applicationService = applicationService;
        On<BookingPaymentRecorded>((ctx, ct) => HandlePayment(ctx.Message, ct));
    }

    Task HandlePayment(BookingPaymentRecorded evt, CancellationToken cancellationToken)
        => _applicationService.Handle(
            new RecordPayment(
                evt.BookingId,
                evt.Amount,
                evt.Currency,
                evt.PaymentId,
                ""
            ),
            cancellationToken
        );
}

static class IntegrationEvents {
    [EventType("BookingPaymentRecorded")]
    public record BookingPaymentRecorded(string PaymentId, string BookingId, float Amount, string Currency);
}