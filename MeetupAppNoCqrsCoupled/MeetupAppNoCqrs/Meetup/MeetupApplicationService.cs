using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetupAppNoCqrs.Domain;
using MeetupAppNoCqrs.Infrastructure;
using MeetupAppNoCqrs.Meetup.Requests;

namespace MeetupAppNoCqrs.Meetup
{
    public class MeetupApplicationService
    {
        private readonly MeetupRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MeetupApplicationService(
            MeetupRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public Domain.Meetup Create(CreateMeetupRequest request)
        {
            var meetup = new Domain.Meetup(0, request.HostUserId, request.Name, request.SeatsAvailable);

            _repository.Add(meetup);
            _unitOfWork.Commit();

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

            _unitOfWork.Commit();

            return result;
        }

        public Result CreateReservation(int meetupId, ReserveSeatRequest request)
        {
            var meetup = _repository.Load(meetupId);
            var result = meetup.ReserveSeat(request.ParticipantUserId, DateTime.UtcNow);

            _unitOfWork.Commit();

            return result;
        }

        public Result CancelReservation(int meetupId, int participantUserId)
        {
            var meetup = _repository.Load(meetupId);
            var result = meetup.CancelSeatReservation(participantUserId);

            _unitOfWork.Commit();

            return result;
        }
    }
}
