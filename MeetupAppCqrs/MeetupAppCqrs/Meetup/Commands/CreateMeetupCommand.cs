using MeetupAppCqrs.Infrastructure.Cqrs;

namespace MeetupAppCqrs.Meetup.Commands
{
    public class CreateMeetupCommand : ICommand<Domain.Meetup>
    {
        public int HostUserId { get; set; }
        public string Name { get; set; }
        public int SeatsAvailable { get; set; }

        public CreateMeetupCommand(int hostUserId, string name, int seatsAvailable)
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

        public CreateMeetupCommandHandler(MeetupRepository repository)
        {
            _repository = repository;
        }

        public Domain.Meetup Handle(CreateMeetupCommand command)
        {
            var meetup = new Domain.Meetup(0, 
                command.HostUserId, 
                command.Name, 
                command.SeatsAvailable);

            _repository.Add(meetup);

            _repository.Commit();

            return meetup;
        }
    }
}
