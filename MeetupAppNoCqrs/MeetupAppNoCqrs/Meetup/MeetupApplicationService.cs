using System;
using MeetupAppNoCqrs.Domain;
using MeetupAppNoCqrs.Meetup.Requests;

namespace MeetupAppNoCqrs.Meetup
{
    public class MeetupApplicationService
    {
        private readonly MeetupRepository _repository;

        public MeetupApplicationService(MeetupRepository repository)
        {
            _repository = repository;
        }

        public Domain.Meetup Create(CreateMeetupRequest request)
        {
            var meetup = new Domain.Meetup(0, request.HostUserId, request.Name, request.SeatsAvailable);

            _repository.Add(meetup);
            _repository.Commit();

            return meetup;
        }

        public Result Update(int meetupId, UpdateMeetupRequest request)
        {
            Result result = Result.Successful;

            var meetup = _repository.Load(meetupId);

            if (!meetup.Name.Equals(request.Name, StringComparison.InvariantCulture))
            {
                result = meetup.UpdateName(request.Name);
                if (!result.Success)
                    return result;
            }
            if (meetup.TotalAvailableSeats != request.SeatsAvailable)
            {
                result = meetup.UpdateAvailableSeats(request.SeatsAvailable);
                if (!result.Success)
                    return result;
            }

            _repository.Commit();

            return result;
        }

        public Result CreateReservation(int meetupId, ReserveSeatRequest request)
        {
            var meetup = _repository.Load(meetupId);
            var result = meetup.ReserveSeat(request.ParticipantUserId, DateTime.UtcNow);

            _repository.Commit();

            return result;
        }

        public Result CancelReservation(int meetupId, int participantUserId)
        {
            var meetup = _repository.Load(meetupId);
            var result = meetup.CancelSeatReservation(participantUserId);

            _repository.Commit();

            return result;
        }
    }
}
