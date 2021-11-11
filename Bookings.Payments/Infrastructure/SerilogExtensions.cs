using Serilog;
using Serilog.Events;
using Serilog.Extensions.Hosting;
using Serilog.Extensions.Logging;

namespace Bookings.Payments.Infrastructure;

public static class Logging {
    public static IServiceCollection AddSerilog(this IServiceCollection services) {
        var diagnosticContext = new DiagnosticContext(null);

        return services
            .AddSingleton<ILoggerFactory>(_ => new SerilogLoggerFactory())
            .AddSingleton(diagnosticContext)
            .AddSingleton<IDiagnosticContext>(diagnosticContext);
    }

    public static void ConfigureLog() {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Grpc", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}"
            )
            .CreateLogger();
    }
}