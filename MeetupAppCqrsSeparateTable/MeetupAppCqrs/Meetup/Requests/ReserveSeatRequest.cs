using System;

namespace MeetupAppCqrs.Meetup.Requests
{
    public class ReserveSeatRequest
    {
        public Guid ParticipantUserId { get; set; }
    }
}
