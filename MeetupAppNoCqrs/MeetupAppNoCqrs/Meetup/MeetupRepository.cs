using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
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
                .Single(x => x.Id == id);
        }

        public Dtos.MeetupDto LoadWithParticipantsInfo(int id)
        {
            var connection = _dbContext.Database.GetDbConnection();

            Dtos.MeetupDto meetup = null;
            connection.Query<Dtos.MeetupDto, Domain.SeatReservation, Domain.UserProfile, Dtos.MeetupDto>(
                "SELECT m.id, m.name, m.totalAvailableSeats, sr.id, sr.ParticipantUserId, up.id, up.DisplayName FROM Meetups m" +
                " LEFT JOIN SeatReservation sr ON sr.MeetupId = m.id" +
                " LEFT JOIN UserProfiles up ON up.Id = sr.ParticipantUserId" +
                " WHERE m.id = @id",
                (m, sr, up) => {
                    if (meetup == null)
                        meetup = m;

                    if (meetup.Participants == null)
                        meetup.Participants = new List<Dtos.ParticipantDto>();

                    if (sr != null)
                    {
                        meetup.Participants.Add(new Dtos.ParticipantDto
                        {
                            UserId = sr.ParticipantUserId,
                            DisplayName = up?.DisplayName
                        });
                    }

                    return meetup;
                }, new { id });

            return meetup;
        }

        public List<Domain.Meetup> LoadAll()
        {
            return _dbContext.Meetups.ToList();
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
