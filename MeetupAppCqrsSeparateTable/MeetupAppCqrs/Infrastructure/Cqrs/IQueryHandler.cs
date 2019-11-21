namespace MeetupAppCqrs.Infrastructure.Cqrs
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : class, IQuery<TResult>
    {
        TResult Handle(TQuery query);
    }
}
