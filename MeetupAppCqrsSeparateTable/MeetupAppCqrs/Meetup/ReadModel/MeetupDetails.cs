using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using MeetupAppCqrs.Meetup.Dtos;

namespace MeetupAppCqrs.Meetup.ReadModel
{
    public class MeetupDetails
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TotalAvailableSeats { get; set; }
        public string Participants 
        {
            get => JsonSerializer.Serialize(ParticipantsList);
            private set => ParticipantsList = JsonSerializer.Deserialize<List<ParticipantDto>>(value);
        }

        [NotMapped]
        public List<ParticipantDto> ParticipantsList { get; set; }
    }
}
