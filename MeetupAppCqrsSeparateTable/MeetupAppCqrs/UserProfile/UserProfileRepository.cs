using System;
using System.Collections.Generic;
using System.Linq;
using MeetupAppCqrs.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MeetupAppCqrs.UserProfile
{
    public class UserProfileRepository : IDisposable
    {
        private readonly MeetupAppDbContext _dbContext;

        public UserProfileRepository(MeetupAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guid GetNextId()
        {
            return Guid.NewGuid();
        }

        public void Add(Domain.UserProfile userProfile)
        {
            _dbContext.UserProfiles.Add(userProfile);
        }

        public Domain.UserProfile Load(Guid id)
        {
            return _dbContext.UserProfiles
                .Include(x => x.Friends)
                .Single(x => x.Id == id);
        }

        public List<Domain.UserProfile> LoadRange(IEnumerable<Guid> ids)
        {
            return _dbContext.UserProfiles
                .Where(x => ids.Contains(x.Id))
                .ToList();
        }

        public bool Exists(Guid id)
        {
            return _dbContext.UserProfiles.Any(x => x.Id == id);
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
