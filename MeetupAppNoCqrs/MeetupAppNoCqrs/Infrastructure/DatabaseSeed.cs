using System;

namespace MeetupAppNoCqrs.Infrastructure
{
    public static class DatabaseSeed
    {
        public static void Seed(MeetupAppDbContext dbContext)
        {
            dbContext.Meetups.RemoveRange(dbContext.Meetups);
            dbContext.UserProfiles.RemoveRange(dbContext.UserProfiles);

            dbContext.SaveChanges();

            var user1 = new Domain.UserProfile(1, "Helmut");
            var user2 = new Domain.UserProfile(2, "Gertruda");
            var user3 = new Domain.UserProfile(3, "Kunegunda");
            var user4 = new Domain.UserProfile(4, "Hans");

            dbContext.UserProfiles.AddRange(new[] { user1, user2, user3, user4 });
            dbContext.SaveChanges();

            user1.AddFriend(user2.Id);
            user1.AddFriend(user3.Id);

            dbContext.SaveChanges();

            var meetup1 = new Domain.Meetup(1, user4.Id, "Literatura Lovecrafta", 15);
            var meetup2 = new Domain.Meetup(2, user4.Id, "Komiks dzisiaj i w przeszlosci", 15);
            var meetup3 = new Domain.Meetup(3, user4.Id, "Dlaczego Mroczne Widmo to najlepsza czesc Gwiezdnych Wojen", 15);
            var meetup4 = new Domain.Meetup(4, user4.Id, "Mitologia w literaturze Fantasy", 15);

            dbContext.Meetups.AddRange(new[] { meetup1, meetup2, meetup3, meetup4 });
            dbContext.SaveChanges();

            meetup1.ReserveSeat(user2.Id, DateTimeOffset.Now);
            meetup2.ReserveSeat(user3.Id, DateTimeOffset.Now);
            meetup3.ReserveSeat(user2.Id, DateTimeOffset.Now);
            meetup3.ReserveSeat(user3.Id, DateTimeOffset.Now);

            dbContext.SaveChanges();
        }
    }
}
