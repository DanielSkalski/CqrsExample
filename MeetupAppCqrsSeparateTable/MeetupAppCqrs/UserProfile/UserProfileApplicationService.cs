using System;
using MeetupAppCqrs.UserProfile.Requests;

namespace MeetupAppCqrs.UserProfile
{
    public class UserProfileApplicationService
    {
        private readonly UserProfileRepository _repository;
        private readonly Meetup.MeetupReadModelUpdater _meetupReadModelUpdater;

        public UserProfileApplicationService(
            UserProfileRepository repository,
            Meetup.MeetupReadModelUpdater meetupReadModelUpdater)
        {
            _repository = repository;
            _meetupReadModelUpdater = meetupReadModelUpdater;
        }

        public Domain.UserProfile Create(CreateUserProfileRequest request)
        {
            var userId = _repository.GetNextId();
            var userProfile = new Domain.UserProfile(userId, request.DisplayName);

            _repository.Add(userProfile);
            _repository.Commit();

            return userProfile;
        }

        public Domain.Result Update(Guid id, UpdateUserProfileRequest request)
        {
            var userProfile = _repository.Load(id);
            var result = userProfile.UpdateDisplayName(request.DisplayName);
            _meetupReadModelUpdater.OnUserDisplayNameUpdated(userProfile);
            _repository.Commit();
            return result;
        }

        public void AddFriend(Guid id, AddFriendRequest request)
        {
            var userProfile = _repository.Load(id);
            var friendUserProfile = _repository.Load(request.FriendUserId);

            userProfile.AddFriend(request.FriendUserId);
            friendUserProfile.AddFriend(id);

            _repository.Commit();
        }

        public void RemoveFriend(Guid id, Guid friendUserId)
        {
            var userProfile = _repository.Load(id);
            var friendUserProfile = _repository.Load(friendUserId);

            userProfile.RemoveFriend(friendUserId);
            friendUserProfile.RemoveFriend(id);

            _repository.Commit();
        }
    }
}
