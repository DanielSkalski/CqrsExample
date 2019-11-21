using MeetupAppCqrs.Infrastructure.Cqrs;

namespace MeetupAppCqrs.Meetup.Commands
{
    public class CancelSeatReservationCommand : ICommand
    {
        public int MeetupId { get; set; }
        public int ParticipantUserId { get; set; }

        public CancelSeatReservationCommand(int meetupId, int participantUserId)
        {
            MeetupId = meetupId;
            ParticipantUserId = participantUserId;
        }
    }

    public class CancelSeatReservationCommandHandler : ICommandHandler<CancelSeatReservationCommand>
    {
        private readonly MeetupRepository _repository;

        public CancelSeatReservationCommandHandler(MeetupRepository repository)
        {
            _repository = repository;
        }

        public void Handle(CancelSeatReservationCommand command)
        {
            var meetup = _repository.Load(command.MeetupId);

            meetup.CancelSeatReservation(command.ParticipantUserId);

            _repository.Commit();
        }
    }
}
