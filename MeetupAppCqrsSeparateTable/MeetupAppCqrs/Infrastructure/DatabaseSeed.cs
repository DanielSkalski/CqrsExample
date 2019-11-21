using System;
using System.Collections.Generic;
using MeetupAppCqrs.Meetup.Dtos;

namespace MeetupAppCqrs.Infrastructure
{
    public static class DatabaseSeed
    {
        public static void Seed(MeetupAppDbContext dbContext)
        {
            dbContext.Meetups.RemoveRange(dbContext.Meetups);
            dbContext.UserProfiles.RemoveRange(dbContext.UserProfiles);
            dbContext.MeetupDetailsReadModel.RemoveRange(dbContext.MeetupDetailsReadModel);
            dbContext.UserSeatReservationsReadModel.RemoveRange(dbContext.UserSeatReservationsReadModel);
            dbContext.SaveChanges();

            var userIds = new[]
            {
                Guid.Parse("7e3335a5-b1bf-4849-8223-bf910195a3e8"),
                Guid.Parse("f215f457-d9f4-451d-9099-a320c6e5f76b"),
                Guid.Parse("d3f5ecec-6270-4fd6-b431-8e0e8a5b8543"),
                Guid.Parse("282e3bd3-eabd-42d0-8b43-8f7a03c0ec8a")
            };

            var user1 = new Domain.UserProfile(userIds[0], "Helmut");
            var user2 = new Domain.UserProfile(userIds[1], "Gertruda");
            var user3 = new Domain.UserProfile(userIds[2], "Kunegunda");
            var user4 = new Domain.UserProfile(userIds[3], "Hans");

            dbContext.UserProfiles.AddRange(new[] { user1, user2, user3, user4 });
            dbContext.SaveChanges();

            user1.AddFriend(user2.Id);
            user1.AddFriend(user3.Id);
            dbContext.SaveChanges();

            var meetupIds = new[]
            {
                Guid.Parse("b107d2cc-7e01-4bd3-ba1b-1195cab7ab66"),
                Guid.Parse("a6cce458-6d6f-48e3-b4b3-ab5a38a46b30"),
                Guid.Parse("bf545ec3-76e3-4599-9295-7295c14f79dc"),
                Guid.Parse("acd2f929-26e2-4e7d-8ad6-5d5aadafd68d"),
            };

            var meetup1 = new Domain.Meetup(meetupIds[0], user4.Id, "Literatura Lovecrafta", 15);
            var meetup2 = new Domain.Meetup(meetupIds[1], user4.Id, "Komiks dzisiaj i w przeszlosci", 15);
            var meetup3 = new Domain.Meetup(meetupIds[2], user4.Id, "Dlaczego Mroczne Widmo to najlepsza czesc Gwiezdnych Wojen", 15);       
            var meetup4 = new Domain.Meetup(meetupIds[3], user4.Id, "Mitologia w literaturze Fantasy", 15);

            dbContext.Meetups.AddRange(new[] { meetup1, meetup2, meetup3, meetup4 });
            dbContext.SaveChanges();

            meetup1.ReserveSeat(user2.Id, DateTimeOffset.Now);
            meetup2.ReserveSeat(user3.Id, DateTimeOffset.Now);
            meetup3.ReserveSeat(user2.Id, DateTimeOffset.Now);
            meetup3.ReserveSeat(user3.Id, DateTimeOffset.Now);
            dbContext.SaveChanges();

            dbContext.MeetupDetailsReadModel.AddRange(new[]
            {
                new Meetup.ReadModel.MeetupDetails
                {
                    Id = meetup1.Id,
                    Name = meetup1.Name,
                    TotalAvailableSeats = meetup1.TotalAvailableSeats,
                    ParticipantsList = new List<ParticipantDto> 
                    { 
                        new ParticipantDto { UserId = user2.Id, DisplayName = user2.DisplayName }
                    }
                },
                new Meetup.ReadModel.MeetupDetails
                {
                    Id = meetup2.Id,
                    Name = meetup2.Name,
                    TotalAvailableSeats = meetup2.TotalAvailableSeats,
                    ParticipantsList = new List<ParticipantDto>
                    {
                        new ParticipantDto { UserId = user3.Id, DisplayName = user3.DisplayName }
                    }
                },
                new Meetup.ReadModel.MeetupDetails
                {
                    Id = meetup3.Id,
                    Name = meetup3.Name,
                    TotalAvailableSeats = meetup3.TotalAvailableSeats,
                    ParticipantsList = new List<ParticipantDto>
                    {
                        new ParticipantDto { UserId = user2.Id, DisplayName = user2.DisplayName },
                        new ParticipantDto { UserId = user3.Id, DisplayName = user3.DisplayName }
                    }
                },
                new Meetup.ReadModel.MeetupDetails
                {
                    Id = meetup4.Id,
                    Name = meetup4.Name,
                    TotalAvailableSeats = meetup4.TotalAvailableSeats,
                    ParticipantsList = new List<ParticipantDto>()
                },
            });

            dbContext.UserSeatReservationsReadModel.AddRange(new[] 
            { 
                new Meetup.ReadModel.UserSeatReservations{ UserId = user1.Id, SeatReservationsList = new List<Meetup.ReadModel.UserSeatReservationData>() },
                new Meetup.ReadModel.UserSeatReservations{ UserId = user4.Id, SeatReservationsList = new List<Meetup.ReadModel.UserSeatReservationData>() },
                new Meetup.ReadModel.UserSeatReservations
                {
                    UserId = user2.Id, SeatReservationsList = new List<Meetup.ReadModel.UserSeatReservationData>
                    {
                        new Meetup.ReadModel.UserSeatReservationData{ MeetupId = meetup1.Id, MeetupName = meetup1.Name },
                        new Meetup.ReadModel.UserSeatReservationData{ MeetupId = meetup3.Id, MeetupName = meetup3.Name }
                    }
                },
                new Meetup.ReadModel.UserSeatReservations
                {
                    UserId = user3.Id, SeatReservationsList = new List<Meetup.ReadModel.UserSeatReservationData>
                    {
                        new Meetup.ReadModel.UserSeatReservationData { MeetupId = meetup2.Id, MeetupName = meetup2.Name },
                        new Meetup.ReadModel.UserSeatReservationData { MeetupId = meetup3.Id, MeetupName = meetup3.Name }
                    }
                }
            });
            dbContext.SaveChanges();
        }
    }
}
