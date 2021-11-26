using Bookings.Payments.Application;
using Bookings.Payments.Domain;
using Bookings.Payments.Infrastructure;
using Bookings.Payments.Integration;
using Eventuous.Diagnostics.OpenTelemetry;
using Eventuous.EventStore;
using Eventuous.EventStore.Producers;
using Eventuous.EventStore.Subscriptions;
using Eventuous.Producers;
using Eventuous.Projections.MongoDB;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Bookings.Payments; 

public static class Registrations {
    public static void AddServices(this IServiceCollection services) {
        services.AddEventStoreClient("esdb://localhost:2113?tls=false");
        services.AddAggregateStore<EsdbEventStore>();
        services.AddApplicationService<CommandService, Payment>();
        services.AddSingleton(Mongo.ConfigureMongo());
        services.AddCheckpointStore<MongoCheckpointStore>();
        services.AddEventProducer<EventStoreProducer>();

        services
            .AddShovel<AllStreamSubscription, AllStreamSubscriptionOptions, EventStoreProducer>(
                "IntegrationSubscription",
                PaymentsShovel.Transform
            );
    }
    
    public static void AddOpenTelemetry(this IServiceCollection services) {
        services.AddOpenTelemetryMetrics(
            builder => builder
                .AddAspNetCoreInstrumentation()
                .AddEventuous()
                .AddEventuousSubscriptions()
                .AddPrometheusExporter()
        );
        services.AddOpenTelemetryTracing(
            builder => builder
                .AddAspNetCoreInstrumentation()
                .AddGrpcClientInstrumentation()
                .AddEventuousTracing()
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("payments"))
                .SetSampler(new AlwaysOnSampler())
                .AddZipkinExporter()
        );
    }
}