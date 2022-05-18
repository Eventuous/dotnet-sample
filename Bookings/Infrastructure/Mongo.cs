using MongoDb.Bson.NodaTime;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;

namespace Bookings.Infrastructure;

public static class Mongo {
    public static IMongoDatabase ConfigureMongo(IConfiguration configuration) {
        NodaTimeSerializers.Register();
        var settings = MongoClientSettings.FromConnectionString(configuration["Mongo:ConnectionString"]);
        settings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());
        return new MongoClient(settings).GetDatabase(configuration["Mongo:Database"]);
    }
}