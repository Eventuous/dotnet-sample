using Microsoft.AspNetCore.Mvc;
using static Bookings.Payments.Application.PaymentCommands;

namespace Bookings.Payments.Application; 

[Route("payment")]
public class CommandApi : ControllerBase {
    readonly CommandService _service;
        
    public CommandApi(CommandService service) => _service = service;

    [HttpPost]
    public Task RegisterPayment([FromBody] RecordPayment cmd, CancellationToken cancellationToken)
        => _service.Handle(cmd, cancellationToken);
}