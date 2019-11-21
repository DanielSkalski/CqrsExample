using System;
using MeetupAppCqrs.Domain;
using MeetupAppCqrs.Infrastructure.Cqrs;

namespace MeetupAppCqrs.Meetup.Commands
{
    public class ReserveSeatCommand : ICommand<Result>
    {
        public int MeetupId { get; }
        public int ParticipantUserId { get; }

        public ReserveSeatCommand(int meetupId, int participantUserId)
        {
            MeetupId = meetupId;
            ParticipantUserId = participantUserId;
        }
    }

    public class ReserveSeatCommandHandler : ICommandHandler<ReserveSeatCommand, Result>
    {
        private readonly MeetupRepository _repository;

        public ReserveSeatCommandHandler(MeetupRepository meetupRepository)
        {
            _repository = meetupRepository;
        }

        public Result Handle(ReserveSeatCommand command)
        {
            var meetup = _repository.Load(command.MeetupId);

            var result = meetup.ReserveSeat(command.ParticipantUserId, DateTime.UtcNow);

            _repository.Commit();

            return result;
        }
    }
}
