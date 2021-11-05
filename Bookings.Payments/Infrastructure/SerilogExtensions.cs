using Serilog;
using Serilog.Extensions.Hosting;
using Serilog.Extensions.Logging;

namespace Bookings.Payments.Infrastructure;

public static class SerilogExtensions {
    public static IServiceCollection AddSerilog(this IServiceCollection services) {
        var diagnosticContext = new DiagnosticContext(null);

        return services
            .AddSingleton<ILoggerFactory>(_ => new SerilogLoggerFactory())
            .AddSingleton(diagnosticContext)
            .AddSingleton<IDiagnosticContext>(diagnosticContext);
    }
}