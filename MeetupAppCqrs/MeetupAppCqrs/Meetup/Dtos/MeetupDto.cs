using System.Collections.Generic;

namespace MeetupAppCqrs.Meetup.Dtos
{
    public class MeetupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ParticipantDto> Participants { get; set; }
        public int TotalAvailableSeats { get; set; }
    }
}
