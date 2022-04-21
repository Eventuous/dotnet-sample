using Bookings.Payments.Domain;
using Eventuous;
using Eventuous.Gateway;
using Eventuous.Subscriptions.Context;

namespace Bookings.Payments.Integration;

public static class PaymentsGateway {
    static readonly StreamName Stream = new("PaymentsIntegration");
    
    public static ValueTask<GatewayContext?> Transform(IMessageConsumeContext original) {
        var result = original.Message is PaymentEvents.PaymentRecorded evt
            ? new GatewayContext(
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