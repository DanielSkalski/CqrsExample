using System;
using MeetupAppCqrs.Domain;
using MeetupAppCqrs.Infrastructure.Cqrs;

namespace MeetupAppCqrs.Meetup.Commands
{
    public class UpdateMeetupCommand : ICommand<Result>
    {
        public Guid MeetupId { get; }
        public string Name { get; }
        public int SeatsAvailable { get; }

        public UpdateMeetupCommand(Guid meetupId, string name, int seatsAvailable)
        {
            MeetupId = meetupId;
            Name = name;
            SeatsAvailable = seatsAvailable;
        }
    }

    public class UpdateMeetupCommandHandler : ICommandHandler<UpdateMeetupCommand, Result>
    {
        private readonly MeetupRepository _repository;
        private readonly MeetupReadModelUpdater _readModelUpdater;

        public UpdateMeetupCommandHandler(
            MeetupRepository repository,
            MeetupReadModelUpdater readModelUpdater)
        {
            _repository = repository;
            _readModelUpdater = readModelUpdater;
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

            _readModelUpdater.OnUpdated(meetup);

            _repository.Commit();

            return result;
        }
    }
}
