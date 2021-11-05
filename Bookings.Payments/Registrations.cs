using Bookings.Payments.Application;
using Bookings.Payments.Domain;
using Bookings.Payments.Infrastructure;
using Bookings.Payments.Integration;
using Eventuous;
using Eventuous.Diagnostics.OpenTelemetry;
using Eventuous.Diagnostics.Registrations;
using Eventuous.EventStore;
using Eventuous.EventStore.Producers;
using Eventuous.EventStore.Subscriptions;
using Eventuous.Producers;
using Eventuous.Projections.MongoDB;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Bookings.Payments; 

public static class Registrations {
    public static void AddServices(this IServiceCollection services) {
        services.AddEventStoreClient("esdb://localhost:2113?tls=false");
        services.AddEventStore<EsdbEventStore>();
        services.AddApplicationService<CommandService, PaymentState, PaymentId>();
        services.AddSingleton<IAggregateStore, AggregateStore>();

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