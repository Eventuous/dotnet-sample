using Eventuous;
using static Bookings.Payments.Domain.PaymentEvents;

namespace Bookings.Payments.Domain;

public class Payment : Aggregate<PaymentState, PaymentId> {
    public void ProcessPayment(
        PaymentId paymentId, string bookingId, float amount, string method, string provider, string currency
    ) {
        Apply(new PaymentRecorded(paymentId, bookingId, amount, method, provider, currency));
    }
}

public record PaymentState : AggregateState<PaymentState, PaymentId> {
    public string BookingId { get; init; }
    public float  Amount    { get; init; }

    public PaymentState() {
        On<PaymentRecorded>(
            (state, recorded) => state with {
                Id = new PaymentId(recorded.PaymentId),
                BookingId = recorded.BookingId,
                Amount = recorded.Amount
            }
        );
    }
}

public record PaymentId(string Value) : AggregateId(Value);