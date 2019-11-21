using System.Collections.Generic;
using Dapper;
using MeetupAppCqrs.Infrastructure;
using MeetupAppCqrs.Infrastructure.Cqrs;
using Microsoft.EntityFrameworkCore;

namespace MeetupAppCqrs.Meetup.Queries
{
    public class GetMeetupQuery : IQuery<Dtos.MeetupDto>
    {
        public int Id { get; set; }

        public GetMeetupQuery(int id) => Id = id;
        public GetMeetupQuery() { }
    }

    public class GetMeetupQueryHandler : IQueryHandler<GetMeetupQuery, Dtos.MeetupDto>
    {
        private readonly MeetupAppDbContext _dbContext;

        public GetMeetupQueryHandler(MeetupAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Dtos.MeetupDto Handle(GetMeetupQuery query)
        {
            using var connection = _dbContext.Database.GetDbConnection();

            Dtos.MeetupDto meetup = null;

            connection.Query<Dtos.MeetupDto, Domain.SeatReservation, Domain.UserProfile, Dtos.MeetupDto>(
                "SELECT m.id, m.name, m.TotalAvailableSeats, sr.id, sr.ParticipantUserId, up.id, up.DisplayName FROM Meetups m" +
                " LEFT JOIN SeatReservation sr ON sr.MeetupId = m.id" +
                " LEFT JOIN UserProfiles up ON up.Id = sr.ParticipantUserId" +
                " WHERE m.id = @Id",
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
                }, new { query.Id });

            return meetup;
        }
    }
}
