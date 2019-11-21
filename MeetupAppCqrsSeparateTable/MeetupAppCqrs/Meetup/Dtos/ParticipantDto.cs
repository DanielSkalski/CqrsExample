using System;

namespace MeetupAppCqrs.Meetup.Dtos
{
    public class ParticipantDto
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
    }
}
