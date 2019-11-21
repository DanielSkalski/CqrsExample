using System;

namespace MeetupAppCqrs.Meetup.Dtos
{
    public class MeetupListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool AreSeatsAvailable { get; set; }
    }
}
