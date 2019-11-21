using System;
using System.Collections.Generic;
using System.Linq;
using MeetupAppNoCqrs.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MeetupAppNoCqrs.UserProfile
{
    public class UserProfileRepository : IDisposable
    {
        private readonly MeetupAppDbContext _dbContext;

        public UserProfileRepository(MeetupAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Domain.UserProfile userProfile)
        {
            _dbContext.UserProfiles.Add(userProfile);
        }

        public Domain.UserProfile Load(int id)
        {
            return _dbContext.UserProfiles
                .Include(x => x.Friends)
                .Single(x => x.Id == id);
        }

        public List<Domain.UserProfile> LoadRange(IEnumerable<int> ids)
        {
            return _dbContext.UserProfiles
                .Where(x => ids.Contains(x.Id))
                .ToList();
        }

        public bool Exists(int id)
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
