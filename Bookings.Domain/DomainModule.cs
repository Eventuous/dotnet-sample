using System.Runtime.CompilerServices;
using Eventuous;

namespace Bookings.Domain; 

class DomainModule {
    [ModuleInitializer]
    internal static void InitializeDomainModule() => TypeMap.RegisterKnownEventTypes();
}