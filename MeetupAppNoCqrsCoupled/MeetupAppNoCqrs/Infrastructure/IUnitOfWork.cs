namespace MeetupAppNoCqrs.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
