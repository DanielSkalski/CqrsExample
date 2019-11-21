using System;
using System.Collections.Generic;
using System.Linq;
using MeetupAppNoCqrs.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MeetupAppNoCqrs.Meetup
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
                    .ThenInclude(x => x.ParticipantUser)
                .Single(x => x.Id == id);
        }

        public List<Domain.Meetup> LoadAll()
        {
            return _dbContext.Meetups.ToList();
        }

        public void Add(Domain.Meetup meetup)
        {
            _dbContext.Meetups.Add(meetup);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
