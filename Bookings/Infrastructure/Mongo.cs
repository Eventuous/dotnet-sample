using MongoDb.Bson.NodaTime;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;

namespace Bookings.Infrastructure;

public static class Mongo {
    public static IMongoDatabase ConfigureMongo() {
        NodaTimeSerializers.Register();
        var settings = MongoClientSettings.FromConnectionString("mongodb://mongoadmin:secret@localhost:27017");
        settings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());
        return new MongoClient(settings).GetDatabase("bookings");
    }
}