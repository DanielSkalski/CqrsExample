using System;
using MeetupAppCqrs.UserProfile.Requests;
using Microsoft.AspNetCore.Mvc;

namespace MeetupAppCqrs.UserProfile
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
        public IActionResult Get(Guid id)
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
        [Route("{id}")]
        public IActionResult Update(Guid id, UpdateUserProfileRequest request)
        {
            var result = _applicationService.Update(id, request);
            if (result.Success)
                return Ok();
            else
                return BadRequest(new { Error = result.ErrorMessage });
        }

        [HttpPut]
        [Route("{id}/friend")]
        public IActionResult AddFriend(Guid id, AddFriendRequest request)
        {
            _applicationService.AddFriend(id, request);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}/friend/{friendUserId}")]
        public IActionResult RemoveFriend(Guid id, Guid friendUserId)
        {
            _applicationService.RemoveFriend(id, friendUserId);
            return Ok();
        }
    }
}
