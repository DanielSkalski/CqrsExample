using System;
using MeetupAppCqrs.Infrastructure.Cqrs;

namespace MeetupAppCqrs.Meetup.Commands
{
    public class CreateMeetupCommand : ICommand<Domain.Meetup>
    {
        public Guid HostUserId { get; set; }
        public string Name { get; set; }
        public int SeatsAvailable { get; set; }

        public CreateMeetupCommand(Guid hostUserId, string name, int seatsAvailable)
        {
            HostUserId = hostUserId;
            Name = name;
            SeatsAvailable = seatsAvailable;
        }

        public CreateMeetupCommand()
        {
        }
    }

    public class CreateMeetupCommandHandler : ICommandHandler<CreateMeetupCommand, Domain.Meetup>
    {
        private readonly MeetupRepository _repository;
        private readonly MeetupReadModelUpdater _readModelUpdater;

        public CreateMeetupCommandHandler(
            MeetupRepository repository, 
            MeetupReadModelUpdater readModelUpdater)
        {
            _repository = repository;
            _readModelUpdater = readModelUpdater;
        }

        public Domain.Meetup Handle(CreateMeetupCommand command)
        {
            var meetupId = _repository.GetNextId();

            var meetup = new Domain.Meetup(
                meetupId,
                command.HostUserId, 
                command.Name, 
                command.SeatsAvailable);

            _repository.Add(meetup);

            _readModelUpdater.OnCreate(meetup);

            _repository.Commit();

            return meetup;
        }
    }
}
