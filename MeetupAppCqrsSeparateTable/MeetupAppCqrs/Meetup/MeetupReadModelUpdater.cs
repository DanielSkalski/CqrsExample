using System;
using System.Collections.Generic;
using System.Linq;
using MeetupAppCqrs.Infrastructure;

namespace MeetupAppCqrs.Meetup
{
    public class MeetupReadModelUpdater
    {
        private readonly MeetupAppDbContext _dbContext;

        public MeetupReadModelUpdater(MeetupAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnCreate(Domain.Meetup meetup)
        {
            _dbContext.MeetupDetailsReadModel.Add(new ReadModel.MeetupDetails 
            { 
                Id = meetup.Id,
                Name = meetup.Name,
                TotalAvailableSeats = meetup.TotalAvailableSeats,
                ParticipantsList = new List<Dtos.ParticipantDto>()
            });
        }

        public void OnSeatReserved(Domain.Meetup meetup, Guid participantUserId)
        {
            var meetupReadModel = _dbContext.MeetupDetailsReadModel.Find(meetup.Id);
            var userProfile = _dbContext.UserProfiles.Find(participantUserId);

            meetupReadModel.ParticipantsList.Add(new Dtos.ParticipantDto
            {
                UserId = userProfile.Id,
                DisplayName = userProfile.DisplayName
            });

            var userReservationsReadModel = _dbContext.UserSeatReservationsReadModel.Find(participantUserId);
            userReservationsReadModel.SeatReservationsList.Add(new ReadModel.UserSeatReservationData(meetup));
        }

        public void OnSeatReservationCanceled(Domain.Meetup meetup, Guid participantUserId)
        {
            var meetupReadModel = _dbContext.MeetupDetailsReadModel.Find(meetup.Id);
            var userProfile = _dbContext.UserProfiles.Find(participantUserId);

            meetupReadModel.ParticipantsList.RemoveAll(x => x.UserId == participantUserId);

            var userReservationsReadModel = _dbContext.UserSeatReservationsReadModel.Find(participantUserId);
            userReservationsReadModel.SeatReservationsList.RemoveAll(x => x.MeetupId == meetup.Id);
        }

        public void OnUpdated(Domain.Meetup meetup)
        {
            var meetupReadModel = _dbContext.MeetupDetailsReadModel.Find(meetup.Id);
            meetupReadModel.Name = meetup.Name;
            meetupReadModel.TotalAvailableSeats = meetup.TotalAvailableSeats;
        }

        public void OnUserDisplayNameUpdated(Domain.UserProfile userProfile)
        {
            var userSeatReservations = _dbContext.UserSeatReservationsReadModel.Find(userProfile.Id);
            var meetupIds = userSeatReservations.SeatReservationsList.Select(x => x.MeetupId);

            var meetupReadModels = _dbContext.MeetupDetailsReadModel.Where(x => meetupIds.Contains(x.Id));
            foreach (var meetupReadModel in meetupReadModels)
            {
                var participantRecord = meetupReadModel.ParticipantsList.FirstOrDefault(x => x.UserId == userProfile.Id);
                if (participantRecord != null)
                {
                    participantRecord.DisplayName = userProfile.DisplayName;
                }
            }
        }
    }
}
