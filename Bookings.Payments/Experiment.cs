using System.Reflection;
using Bookings.Payments.Application;
using Eventuous;

namespace Bookings.Payments; 

public static class Experiment {
    public static IEndpointRouteBuilder MapCommands<T, TAggregate>(this IEndpointRouteBuilder builder, params Assembly[] assemblies) 
    where T : IApplicationService<TAggregate> where TAggregate : Aggregate {
        var assembliesToScan = assemblies.Length == 0
            ? AppDomain.CurrentDomain.GetAssemblies() : assemblies;

        var attributeType = typeof(CommandAttribute);
        foreach (var assembly in assembliesToScan) {
            MapAssemblyCommands(assembly);
        }

        void MapAssemblyCommands(Assembly assembly) {
            var decoratedTypes = assembly.DefinedTypes.Where(
                x => x.IsClass && x.CustomAttributes.Any(a => a.AttributeType == attributeType)
            );

            foreach (var type in decoratedTypes) {
                var attr = type.GetAttribute<CommandAttribute>()!;
                MapCommand(type, attr.Route);
            }
        }

        void MapCommand(Type type, string route) {
        }

        return builder;
    }
    public static T? GetAttribute<T>(this Type type) where T : class
        => Attribute.GetCustomAttribute(type, typeof(T))! as T;

    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute {
        public string Route { get; }

        public CommandAttribute(string route) => Route = route;
    }
}