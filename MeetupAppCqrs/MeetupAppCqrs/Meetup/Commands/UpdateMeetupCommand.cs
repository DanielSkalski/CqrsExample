using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetupAppCqrs.Domain;
using MeetupAppCqrs.Infrastructure.Cqrs;

namespace MeetupAppCqrs.Meetup.Commands
{
    public class UpdateMeetupCommand : ICommand<Result>
    {
        public int MeetupId { get; }
        public string Name { get; }
        public int SeatsAvailable { get; }

        public UpdateMeetupCommand(int meetupId, string name, int seatsAvailable)
        {
            MeetupId = meetupId;
            Name = name;
            SeatsAvailable = seatsAvailable;
        }
    }

    public class UpdateMeetupCommandHandler : ICommandHandler<UpdateMeetupCommand, Result>
    {
        private readonly MeetupRepository _repository;

        public UpdateMeetupCommandHandler(MeetupRepository repository)
        {
            _repository = repository;
        }

        public Result Handle(UpdateMeetupCommand command)
        {
            Result result = Result.Successful;

            var meetup = _repository.Load(command.MeetupId);

            if (!meetup.Name.Equals(command.Name, StringComparison.InvariantCulture))
            {
                result = meetup.UpdateName(command.Name);
                if (!result.Success)
                    return result;
            }
            if (meetup.TotalAvailableSeats != command.SeatsAvailable)
            {
                result = meetup.UpdateAvailableSeats(command.SeatsAvailable);
                if (!result.Success)
                    return result;
            }

            _repository.Commit();

            return result;
        }
    }
}
