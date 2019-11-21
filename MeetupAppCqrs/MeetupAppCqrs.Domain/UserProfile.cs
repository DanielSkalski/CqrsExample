using System.Collections.Generic;
using System.Linq;

namespace MeetupAppCqrs.Domain
{
    public class UserProfile
    {
        public int Id { get; private set; }
        public string DisplayName { get; private set; }

        private List<FriendLink> _friends = new List<FriendLink>();
        public List<FriendLink> Friends 
        {
            get => new List<FriendLink>(_friends);
            private set => _friends = value;
        }

        public UserProfile(int id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }

        internal UserProfile() { }

        public Result AddFriend(int friendUserId)
        {
            if (_friends.All(x => x.FriendUserId != friendUserId))
            {
                _friends.Add(new FriendLink(friendUserId));
            }

            return Result.Successful;
        }

        public Result RemoveFriend(int friendUserId)
        {
            _friends.RemoveAll(x => x.FriendUserId == friendUserId);

            return Result.Successful;
        }
    }

    public class FriendLink
    {
        public int Id { get; private set; }
        public int FriendUserId { get; private set; }

        internal FriendLink(int friendUserId)
        {
            FriendUserId = friendUserId;
        }

        internal FriendLink() { }
    }
}
