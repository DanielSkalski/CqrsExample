namespace MeetupAppCqrs.Infrastructure.Cqrs
{
    public interface IQueryDispatcher
    {
        TResult Query<TQuery, TResult>(TQuery query) where TQuery : class, IQuery<TResult>;
    }
}
