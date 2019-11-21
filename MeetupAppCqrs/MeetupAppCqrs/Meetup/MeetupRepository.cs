using System;
using System.Linq;
using MeetupAppCqrs.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MeetupAppCqrs.Meetup
{
    public class MeetupRepository : IDisposable
    {
        private readonly MeetupAppDbContext _dbContext;

        public MeetupRepository(MeetupAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Domain.Meetup Load(int id)
        {
            return _dbContext.Meetups
                .Include(x => x.SeatReservations)
                .Single(x => x.Id == id);
        }

        public void Add(Domain.Meetup meetup)
        {
            _dbContext.Meetups.Add(meetup);
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
