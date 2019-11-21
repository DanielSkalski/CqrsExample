using Microsoft.Extensions.DependencyInjection;

namespace MeetupAppCqrs.Infrastructure.Cqrs
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public QueryDispatcher(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public TResult Query<TQuery, TResult>(TQuery query) 
            where TQuery : class, IQuery<TResult>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
            
            return handler.Handle(query);
        }
    }
}
