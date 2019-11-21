using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetupAppNoCqrs.Domain;
using MeetupAppNoCqrs.Infrastructure;
using MeetupAppNoCqrs.UserProfile.Requests;

namespace MeetupAppNoCqrs.UserProfile
{
    public class UserProfileApplicationService
    {
        private readonly UserProfileRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UserProfileApplicationService(
            UserProfileRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public Domain.UserProfile Create(CreateUserProfileRequest request)
        {
            var userProfile = new Domain.UserProfile(0, request.DisplayName);

            _repository.Add(userProfile);
            _unitOfWork.Commit();

            return userProfile;
        }

        public void AddFriend(int id, AddFriendRequest request)
        {
            var userProfile = _repository.Load(id);
            var friendUserProfile = _repository.Load(request.FriendUserId);

            userProfile.AddFriend(request.FriendUserId);
            friendUserProfile.AddFriend(id);

            _unitOfWork.Commit();
        }

        public void RemoveFriend(int id, int friendUserId)
        {
            var userProfile = _repository.Load(id);
            var friendUserProfile = _repository.Load(friendUserId);

            userProfile.RemoveFriend(friendUserId);
            friendUserProfile.RemoveFriend(id);

            _unitOfWork.Commit();
        }
    }
}
