using Eventuous;

namespace Bookings.Domain; 

public record StayPeriod {
    public DateTimeOffset CheckIn  { get; }
    public DateTimeOffset CheckOut { get; }

    internal StayPeriod() { }

    public StayPeriod(DateTimeOffset checkIn, DateTimeOffset checkOut) {
        if (checkIn > checkOut) throw new DomainException("Check in date must be before check out date");

        (CheckIn, CheckOut) = (checkIn, checkOut);
    }
}