using Bookings.Domain.Bookings;
using Eventuous;
using Microsoft.AspNetCore.Mvc;
using static Bookings.Application.Bookings.BookingCommands;

namespace Bookings.HttpApi.Bookings;

[Route("/booking")]
public class CommandApi : ControllerBase {
    readonly IApplicationService<Booking> _service;

    public CommandApi(IApplicationService<Booking> service) => _service = service;

    [HttpPost]
    [Route("book")]
    public Task BookRoom([FromBody] BookRoom cmd, CancellationToken cancellationToken)
        => _service.Handle(cmd, cancellationToken);

    [HttpPost]
    [Route("recordPayment")]
    public Task<Result> RecordPayment([FromBody] RecordPayment cmd, CancellationToken cancellationToken)
        => _service.Handle(cmd, cancellationToken);
}