using System;
using MeetupAppCqrs.Infrastructure.Cqrs;

namespace MeetupAppCqrs.Meetup.Commands
{
    public class CancelSeatReservationCommand : ICommand
    {
        public Guid MeetupId { get; set; }
        public Guid ParticipantUserId { get; set; }

        public CancelSeatReservationCommand(Guid meetupId, Guid participantUserId)
        {
            MeetupId = meetupId;
            ParticipantUserId = participantUserId;
        }
    }

    public class CancelSeatReservationCommandHandler : ICommandHandler<CancelSeatReservationCommand>
    {
        private readonly MeetupRepository _repository;
        private readonly MeetupReadModelUpdater _readModelUpdater;

        public CancelSeatReservationCommandHandler(
            MeetupRepository repository,
            MeetupReadModelUpdater readModelUpdater)
        {
            _repository = repository;
            _readModelUpdater = readModelUpdater;
        }

        public void Handle(CancelSeatReservationCommand command)
        {
            var meetup = _repository.Load(command.MeetupId);

            meetup.CancelSeatReservation(command.ParticipantUserId);

            _readModelUpdater.OnSeatReservationCanceled(meetup, command.ParticipantUserId);

            _repository.Commit();
        }
    }
}
