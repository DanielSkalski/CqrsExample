using MeetupAppNoCqrs.UserProfile.Requests;
using Microsoft.AspNetCore.Mvc;

namespace MeetupAppNoCqrs.UserProfile
{
    [Route("userprofile")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly UserProfileApplicationService _applicationService;
        private readonly UserProfileRepository _repository;

        public UserProfileController(
            UserProfileApplicationService applicationService,
            UserProfileRepository repository)
        {
            _applicationService = applicationService;
            _repository = repository;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var userProfile = _repository.Load(id);
            return Ok(userProfile);
        }

        [HttpPost]
        public IActionResult Create(CreateUserProfileRequest request)
        {
            var userProfile = _applicationService.Create(request);
            return CreatedAtAction(nameof(Get), new { id = userProfile.Id }, userProfile);
        }

        [HttpPut]
        [Route("{id}/friend")]
        public IActionResult AddFriend(int id, AddFriendRequest request)
        {
            _applicationService.AddFriend(id, request);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}/friend/{friendUserId}")]
        public IActionResult RemoveFriend(int id, int friendUserId)
        {
            _applicationService.RemoveFriend(id, friendUserId);
            return Ok();
        }
    }
}
