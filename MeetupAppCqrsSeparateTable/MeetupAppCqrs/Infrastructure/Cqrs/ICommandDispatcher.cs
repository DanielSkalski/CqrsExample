namespace MeetupAppCqrs.Infrastructure.Cqrs
{
    public interface ICommandDispatcher
    {
        void Handle<TCommand>(TCommand command) where TCommand : class, ICommand;
        TResult Handle<TCommand, TResult>(TCommand command) where TCommand : class, ICommand<TResult>;
    }
}
