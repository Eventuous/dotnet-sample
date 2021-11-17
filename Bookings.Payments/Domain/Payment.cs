using Eventuous;
using static Bookings.Payments.Domain.PaymentEvents;

namespace Bookings.Payments.Domain;

public class Payment : Aggregate<PaymentState, PaymentId> {
    public void ProcessPayment(
        PaymentId paymentId, string bookingId, Money amount, string method, string provider
    )
        => Apply(new PaymentRecorded(paymentId, bookingId, amount.Amount, amount.Currency, method, provider));
}

public record PaymentState : AggregateState<PaymentState, PaymentId> {
    public string BookingId { get; init; } = null!;
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

public record PaymentId : AggregateId {
    public PaymentId(string value) : base(value) { }
}