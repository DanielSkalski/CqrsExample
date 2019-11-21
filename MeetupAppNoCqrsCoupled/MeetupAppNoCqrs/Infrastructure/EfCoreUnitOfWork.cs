namespace MeetupAppNoCqrs.Infrastructure
{
    public class EfCoreUnitOfWork : IUnitOfWork
    {
        private readonly MeetupAppDbContext _dbContext;

        public EfCoreUnitOfWork(MeetupAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
