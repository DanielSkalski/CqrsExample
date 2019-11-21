using System.Collections.Generic;
using System.Linq;
using MeetupAppCqrs.Infrastructure;
using MeetupAppCqrs.Infrastructure.Cqrs;
using MeetupAppCqrs.Meetup.Dtos;

namespace MeetupAppCqrs.Meetup.Queries
{
    public class GetMeetupsListQuery : IQuery<ICollection<MeetupListDto>>
    {
    }

    public class GetMeetupsListQueryHandler : 
        IQueryHandler<GetMeetupsListQuery, ICollection<MeetupListDto>>
    {
        private readonly MeetupAppDbContext _dbContext;

        public GetMeetupsListQueryHandler(MeetupAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICollection<MeetupListDto> Handle(GetMeetupsListQuery query)
        {
            var meetups = _dbContext.Meetups.Select(x => new MeetupListDto
            {
                Id = x.Id,
                Name = x.Name,
                AreSeatsAvailable = x.AreSeatsAvailable
            }).ToList();

            return meetups;
        }
    }
}
