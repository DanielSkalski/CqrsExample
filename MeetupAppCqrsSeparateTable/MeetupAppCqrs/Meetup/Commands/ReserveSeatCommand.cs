using System;
using MeetupAppCqrs.Domain;
using MeetupAppCqrs.Infrastructure.Cqrs;

namespace MeetupAppCqrs.Meetup.Commands
{
    public class ReserveSeatCommand : ICommand<Result>
    {
        public Guid MeetupId { get; }
        public Guid ParticipantUserId { get; }

        public ReserveSeatCommand(Guid meetupId, Guid participantUserId)
        {
            MeetupId = meetupId;
            ParticipantUserId = participantUserId;
        }
    }

    public class ReserveSeatCommandHandler : ICommandHandler<ReserveSeatCommand, Result>
    {
        private readonly MeetupRepository _repository;
        private readonly MeetupReadModelUpdater _readModelUpdater;

        public ReserveSeatCommandHandler(
            MeetupRepository meetupRepository, 
            MeetupReadModelUpdater readModelUpdater)
        {
            _repository = meetupRepository;
            _readModelUpdater = readModelUpdater;
        }

        public Result Handle(ReserveSeatCommand command)
        {
            var meetup = _repository.Load(command.MeetupId);

            var result = meetup.ReserveSeat(command.ParticipantUserId, DateTime.UtcNow);

            _readModelUpdater.OnSeatReserved(meetup, command.ParticipantUserId);

            _repository.Commit();

            return result;
        }
    }
}
