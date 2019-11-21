namespace MeetupAppNoCqrs.Meetup.Requests
{
    public class CreateMeetupRequest
    {
        public int HostUserId { get; set; }
        public string Name { get; set; }
        public int SeatsAvailable { get; set; }
    }
}
