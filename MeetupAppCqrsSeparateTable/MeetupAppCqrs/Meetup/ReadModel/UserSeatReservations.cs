using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace MeetupAppCqrs.Meetup.ReadModel
{
    public class UserSeatReservations
    {
        [Key]
        public Guid UserId { get; set; }
        public string SeatReservations 
        {
            get => JsonSerializer.Serialize(SeatReservationsList);
            private set => SeatReservationsList = JsonSerializer.Deserialize<List<UserSeatReservationData>>(value);
        }

        [NotMapped]
        public List<UserSeatReservationData> SeatReservationsList { get; set; }
    }

    public class UserSeatReservationData
    {
        public Guid MeetupId { get; set; }
        public string MeetupName { get; set; }

        public UserSeatReservationData()
        {
        }

        public UserSeatReservationData(Domain.Meetup meetup)
        {
            MeetupId = meetup.Id;
            MeetupName = meetup.Name;
        }
    }
}
