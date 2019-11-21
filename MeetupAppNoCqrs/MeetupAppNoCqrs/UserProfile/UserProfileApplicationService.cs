using MeetupAppNoCqrs.UserProfile.Requests;

namespace MeetupAppNoCqrs.UserProfile
{
    public class UserProfileApplicationService
    {
        private readonly UserProfileRepository _repository;

        public UserProfileApplicationService(UserProfileRepository repository)
        {
            _repository = repository;
        }

        public Domain.UserProfile Create(CreateUserProfileRequest request)
        {
            var userProfile = new Domain.UserProfile(0, request.DisplayName);

            _repository.Add(userProfile);
            _repository.Commit();

            return userProfile;
        }

        public void AddFriend(int id, AddFriendRequest request)
        {
            var userProfile = _repository.Load(id);
            var friendUserProfile = _repository.Load(request.FriendUserId);

            userProfile.AddFriend(request.FriendUserId);
            friendUserProfile.AddFriend(id);

            _repository.Commit();
        }

        public void RemoveFriend(int id, int friendUserId)
        {
            var userProfile = _repository.Load(id);
            var friendUserProfile = _repository.Load(friendUserId);

            userProfile.RemoveFriend(friendUserId);
            friendUserProfile.RemoveFriend(id);

            _repository.Commit();
        }
    }
}
