using Eventuous;

namespace Bookings.Payments.Domain;

public static class PaymentEvents {
    [EventType("PaymentRecorded")]
    public record PaymentRecorded(string PaymentId, string BookingId, float Amount, string Method, string Provider, string Currency);
}