using Microsoft.Extensions.DependencyInjection;

namespace MeetupAppCqrs.Infrastructure.Cqrs
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CommandDispatcher(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Handle<TCommand>(TCommand command) 
            where TCommand : class, ICommand
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();

            handler.Handle(command);
        }

        public TResult Handle<TCommand, TResult>(TCommand command) 
            where TCommand : class, ICommand<TResult>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
            
            return handler.Handle(command);
        }
    }
}
