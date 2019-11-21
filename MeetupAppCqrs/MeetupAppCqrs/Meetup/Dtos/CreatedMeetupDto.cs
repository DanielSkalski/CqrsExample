namespace MeetupAppCqrs.Meetup.Dtos
{
    public class CreatedMeetupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AvailableSeats { get; set; }
    }
}
