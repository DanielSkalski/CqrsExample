namespace MeetupAppNoCqrs.Meetup.Dtos
{
    public class MeetupListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool AreSeatsAvailable { get; set; }
    }
}
