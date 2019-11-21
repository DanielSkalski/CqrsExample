using System;

namespace MeetupAppCqrs.UserProfile.Requests
{
    public class AddFriendRequest
    {
        public Guid FriendUserId { get; set; }
    }
}
