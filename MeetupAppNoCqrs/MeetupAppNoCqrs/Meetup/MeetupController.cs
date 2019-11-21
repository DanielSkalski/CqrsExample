﻿using System.Linq;
using MeetupAppNoCqrs.Meetup.Dtos;
using MeetupAppNoCqrs.Meetup.Requests;
using Microsoft.AspNetCore.Mvc;

namespace MeetupAppNoCqrs.Meetup
{
    [Route("meetup")]
    [ApiController]
    public class MeetupController : ControllerBase
    {
        private readonly MeetupApplicationService _applicationService;
        private readonly MeetupRepository _repository;
        private readonly UserProfile.UserProfileRepository _userProfileRepository;

        public MeetupController(
            MeetupApplicationService applicationService,
            UserProfile.UserProfileRepository userProfileRepository,
            MeetupRepository repository
            )
        {
            _applicationService = applicationService;
            _userProfileRepository = userProfileRepository;
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var meetups = _repository.LoadAll();
            var meetupsDtos = meetups.Select(x => new MeetupListDto
            {
                Id = x.Id,
                Name = x.Name,
                AreSeatsAvailable = x.AreSeatsAvailable
            }).ToArray();

            return Ok(meetupsDtos);
        }

        [HttpGet]
        [Route("{id}/using_second_query")]
        public IActionResult GetUsingSecondQuery(int id)
        {
            var meetup = _repository.Load(id);
            var participantsIds = meetup.SeatReservations.Select(x => x.ParticipantUserId);
            var userProfiles = _userProfileRepository.LoadRange(participantsIds)
                .ToDictionary(x => x.Id);

            var meetupDto = new MeetupDto
            {
                Id = meetup.Id,
                Name = meetup.Name,
                TotalAvailableSeats = meetup.TotalAvailableSeats,
                Participants = meetup.SeatReservations.Select(x => new ParticipantDto
                {
                    UserId = x.ParticipantUserId,
                    DisplayName = userProfiles[x.ParticipantUserId].DisplayName
                }).ToList()
            };

            return Ok(meetupDto);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var meetupDto = _repository.LoadWithParticipantsInfo(id);
            return Ok(meetupDto);
        }

        [HttpPost]
        public IActionResult Create(CreateMeetupRequest request)
        {
            var meetup = _applicationService.Create(request);
            var meetupDto = new CreatedMeetupDto
            {
                Id = meetup.Id,
                Name = meetup.Name,
                AvailableSeats = meetup.AvailableSeats
            };

            return CreatedAtAction(nameof(Get), new { id = meetupDto.Id }, meetupDto);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, UpdateMeetupRequest request)
        {
            var result = _applicationService.Update(id, request);
            if (result.Success)
                return Ok();
            else
                return BadRequest(new { error = result.ErrorMessage });
        }

        [HttpPost]
        [Route("{meetupId}/seatreservation")]
        public IActionResult CreateSeatReservation(int meetupId, ReserveSeatRequest request)
        {
            var result = _applicationService.CreateReservation(meetupId, request);
            if (result.Success)
                return CreatedAtAction(nameof(Get), new { id = meetupId }, null);
            else
                return BadRequest(new { error = result.ErrorMessage });
        }

        [HttpDelete]
        [Route("{meetupId}/seatreservation/{participantUserId}")]
        public IActionResult CancelSeatReservation(int meetupId, int participantUserId)
        {
            _applicationService.CancelReservation(meetupId, participantUserId);
            return NoContent();
        }
    }
}
