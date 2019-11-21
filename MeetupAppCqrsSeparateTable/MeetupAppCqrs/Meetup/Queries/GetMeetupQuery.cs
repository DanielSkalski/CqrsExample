using System;
using MeetupAppCqrs.Infrastructure;
using MeetupAppCqrs.Infrastructure.Cqrs;

namespace MeetupAppCqrs.Meetup.Queries
{
    public class GetMeetupQuery : IQuery<Dtos.MeetupDto>
    {
        public Guid Id { get; set; }

        public GetMeetupQuery(Guid id) => Id = id;
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
            var meetup = _dbContext.MeetupDetailsReadModel.Find(query.Id);

            var meetupDto = new Dtos.MeetupDto
            {
                Id = meetup.Id,
                Name = meetup.Name,
                TotalAvailableSeats = meetup.TotalAvailableSeats,
                Participants = meetup.ParticipantsList
            };

            return meetupDto;
        }
    }
}
