using MongoDB.Driver;

namespace Bookings.Payments.Infrastructure;

public static class Mongo {
    public static IMongoDatabase ConfigureMongo() {
        var settings = MongoClientSettings.FromConnectionString("mongodb://mongoadmin:secret@localhost:27017");
        return new MongoClient(settings).GetDatabase("payments");
    }
}