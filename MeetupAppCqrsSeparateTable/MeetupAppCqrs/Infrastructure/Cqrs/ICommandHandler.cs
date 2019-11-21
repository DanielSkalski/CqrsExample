namespace MeetupAppCqrs.Infrastructure.Cqrs
{
    public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
    {
        void Handle(TCommand command);
    }

    public interface ICommandHandler<in TCommand, TResult> where TCommand : class, ICommand<TResult>
    {
        TResult Handle(TCommand command);
    }
}
