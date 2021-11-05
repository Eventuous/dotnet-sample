using Bookings.Payments.Domain;
using EventStore.Client;
using Eventuous;
using Eventuous.EventStore.Producers;
using Eventuous.EventStore.Subscriptions;
using Eventuous.Shovel;
using Eventuous.Subscriptions.Checkpoints;
using Eventuous.Subscriptions.Consumers;
using Eventuous.Subscriptions.Context;

namespace Bookings.Payments.Integration;

public static class PaymentsShovel {
    static readonly StreamName Stream = new("PaymentsIntegration");
    
    public static ValueTask<ShovelContext?> Transform(IMessageConsumeContext original) {
        var result = original.Message is PaymentEvents.PaymentRecorded evt
            ? new ShovelContext(
                Stream,
                new IntegrationEvents.BookingPaymentRecorded(evt.PaymentId, evt.BookingId, evt.Amount, evt.Currency),
                new Metadata()
            )
            : null;
        return ValueTask.FromResult(result);
    }
}

public static class IntegrationEvents {
    [EventType("BookingPaymentRecorded")]
    public record BookingPaymentRecorded(string PaymentId, string BookingId, float Amount, string Currency);
}