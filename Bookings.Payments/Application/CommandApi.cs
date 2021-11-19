using Bookings.Payments.Domain;
using Eventuous;
using Microsoft.AspNetCore.Mvc;
using static Bookings.Payments.Application.PaymentCommands;

namespace Bookings.Payments.Application; 

[Route("payment")]
public class CommandApi : ControllerBase {
    readonly IApplicationService<Payment> _service;
        
    public CommandApi(IApplicationService<Payment> service) => _service = service;

    [HttpPost]
    public Task RegisterPayment([FromBody] RecordPayment cmd, CancellationToken cancellationToken)
        => _service.Handle(cmd, cancellationToken);
}