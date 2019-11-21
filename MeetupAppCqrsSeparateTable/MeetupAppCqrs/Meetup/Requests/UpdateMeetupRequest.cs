namespace MeetupAppCqrs.Meetup.Requests
{
    public class UpdateMeetupRequest
    {
        public string Name { get; set; }
        public int SeatsAvailable { get; set; }
    }
}
