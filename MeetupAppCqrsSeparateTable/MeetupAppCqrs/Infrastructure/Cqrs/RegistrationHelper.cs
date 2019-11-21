using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace MeetupAppCqrs.Infrastructure.Cqrs
{
    public static class RegistrationHelper
    {
        public static void AddQueryHandlers(this IServiceCollection service)
        {
            var queryHandlerType = typeof(IQueryHandler<,>);

            var handlerTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(
                    t => t.GetInterfaces().Any(
                        i => i.IsGenericType && i.GetGenericTypeDefinition() == queryHandlerType)
                );

            foreach (var handlerType in handlerTypes)
            {
                var @interface = handlerType
                    .GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == queryHandlerType);

                service.AddTransient(@interface, handlerType);
            }
        }

        public static void AddCommandHandlers(this IServiceCollection service)
        {
            var commandHandlerType1 = typeof(ICommandHandler<>);
            var commandHandlerType2 = typeof(ICommandHandler<,>);

            var handlerTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(
                    t => t.GetInterfaces().Any(
                        i => i.IsGenericType && 
                            (i.GetGenericTypeDefinition() == commandHandlerType1 ||
                             i.GetGenericTypeDefinition() == commandHandlerType2))
                );

            foreach (var handlerType in handlerTypes)
            {
                var @interface = handlerType
                    .GetInterfaces()
                    .First(i => i.IsGenericType &&
                            (i.GetGenericTypeDefinition() == commandHandlerType1 ||
                             i.GetGenericTypeDefinition() == commandHandlerType2));

                service.AddTransient(@interface, handlerType);
            }
        }
    }
}
