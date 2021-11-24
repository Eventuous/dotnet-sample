using Eventuous;
using NodaTime;

namespace Bookings.Domain.Bookings;

public static class BookingEvents {
    public static class V1 {
        [EventType("V1.RoomBooked")]
        public record RoomBooked(
            string         BookingId,
            string         GuestId,
            string         RoomId,
            LocalDate      CheckInDate,
            LocalDate      CheckOutDate,
            float          BookingPrice,
            float          PrepaidAmount,
            float          OutstandingAmount,
            string         Currency,
            DateTimeOffset BookingDate
        );

        [EventType("V1.PaymentRecorded")]
        public record PaymentRecorded(
            string         BookingId,
            float          PaidAmount,
            float          Outstanding,
            string         Currency,
            string         PaymentId,
            string         PaidBy,
            DateTimeOffset PaidAt
        );

        [EventType("V1.FullyPaid")]
        public record BookingFullyPaid(string BookingId, DateTimeOffset FullyPaidAt);
    }
}