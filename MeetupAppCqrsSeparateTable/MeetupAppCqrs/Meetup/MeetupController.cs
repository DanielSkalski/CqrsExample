using System;
using System.Collections.Generic;
using MeetupAppCqrs.Domain;
using MeetupAppCqrs.Infrastructure.Cqrs;
using MeetupAppCqrs.Meetup.Commands;
using MeetupAppCqrs.Meetup.Dtos;
using MeetupAppCqrs.Meetup.Queries;
using MeetupAppCqrs.Meetup.Requests;
using Microsoft.AspNetCore.Mvc;

namespace MeetupAppCqrs.Meetup
{
    [Route("meetup")]
    [ApiController]
    public class MeetupController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public MeetupController(
            ICommandDispatcher commandDispatcher,
            IQueryDispatcher queryDispatcher
            )
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var meetups = _queryDispatcher
                .Query<GetMeetupsListQuery, ICollection<MeetupListDto>>(
                    new GetMeetupsListQuery());

            return Ok(meetups);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(Guid id)
        {
            var meetupDto = _queryDispatcher
                .Query<GetMeetupQuery, MeetupDto>(
                    new GetMeetupQuery(id));

            return Ok(meetupDto);
        }

        [HttpPost]
        public IActionResult Create(CreateMeetupCommand request)
        {
            var meetup = _commandDispatcher.Handle<CreateMeetupCommand, Domain.Meetup>(request);
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
        public IActionResult Update(Guid id, UpdateMeetupRequest request)
        {
            var result = _commandDispatcher
                .Handle<UpdateMeetupCommand, Result>(
                    new UpdateMeetupCommand(id, request.Name, request.SeatsAvailable));

            if (result.Success)
                return Ok();
            else
                return BadRequest(new { error = result.ErrorMessage });
        }

        [HttpPost]
        [Route("{meetupId}/seatreservation")]
        public IActionResult CreateSeatReservation(Guid meetupId, ReserveSeatRequest request)
        {
            var result = _commandDispatcher
                .Handle<ReserveSeatCommand, Result>(
                    new ReserveSeatCommand(meetupId, request.ParticipantUserId));
            
            if (result.Success)
                return CreatedAtAction(nameof(Get), new { id = meetupId }, null);
            else
                return BadRequest(new { error = result.ErrorMessage });
        }

        [HttpDelete]
        [Route("{meetupId}/seatreservation/{participantUserId}")]
        public IActionResult CancelSeatReservation(Guid meetupId, Guid participantUserId)
        {
            _commandDispatcher.Handle(new CancelSeatReservationCommand(meetupId, participantUserId));
            return NoContent();
        }
    }
}
