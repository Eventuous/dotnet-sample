using Bookings;
using Bookings.Domain.Bookings;
using Eventuous;
using Eventuous.AspNetCore;
using Eventuous.Diagnostics.Logging;
using Eventuous.Spyglass;
using Microsoft.AspNetCore.Http.Json;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using Serilog;
using Serilog.Events;

TypeMap.RegisterKnownEventTypes(typeof(BookingEvents.V1.RoomBooked).Assembly);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Grpc", LogEventLevel.Information)
    .MinimumLevel.Override("Grpc.Net.Client.Internal.GrpcCall", LogEventLevel.Error)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services
    .AddControllers()
    .AddJsonOptions(cfg => cfg.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddTelemetry();
builder.Services.AddEventuous(builder.Configuration);
builder.Services.AddEventuousSpyglass();

builder.Services.Configure<JsonOptions>(options
    => options.SerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb)
);

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseSwagger().UseSwaggerUI();
app.MapControllers();
app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.MapEventuousSpyglass(null);

var factory  = app.Services.GetRequiredService<ILoggerFactory>();
var listener = new LoggingEventListener(factory, "OpenTelemetry");

try {
    var port = Environment.GetEnvironmentVariable("PORT");
    if (string.IsNullOrWhiteSpace(port)) {
        app.Run();
    }
    else {
        var url  = $"http://+:{port}";
        app.Run(url);
    }
    return 0;
}
catch (Exception e) {
    Log.Fatal(e, "Host terminated unexpectedly");
    return 1;
}
finally {
    Log.CloseAndFlush();
    listener.Dispose();
}
