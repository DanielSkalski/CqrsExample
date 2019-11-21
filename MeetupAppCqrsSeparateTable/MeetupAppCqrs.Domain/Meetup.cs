using System;
using System.Collections.Generic;
using System.Linq;

namespace MeetupAppCqrs.Domain
{
    public class Meetup
    {
        public Guid Id { get; private set; }
        public Guid HostUserId { get; private set; }
        public string Name { get; private set; }
        public int TotalAvailableSeats { get; private set; }
        public List<SeatReservation> SeatReservations 
        {
            get => new List<SeatReservation>(_seatReservations);
            private set => _seatReservations = value;
        }

        public Meetup(Guid id, Guid hostUserId, string name, int seatsAvailable)
        {
            Id = id;
            HostUserId = hostUserId;
            Name = name;
            TotalAvailableSeats = seatsAvailable;
        }

        public Result UpdateName(string name)
        {
            Name = name;
            return Result.Successful;
        }

        public Result UpdateAvailableSeats(int seatsCount)
        {
            if (seatsCount <= 0)
            {
                return Result.Failure("seats count must be greater than zero");
            }

            if (seatsCount < _seatReservations.Count)
            {
                var seatReservationsToCancel = _seatReservations
                    .OrderByDescending(x => x.ReservationDate)
                    .Take(_seatReservations.Count - seatsCount)
                    .ToList();

                foreach (var seatReservation in seatReservationsToCancel)
                {
                    CancelSeatReservation(seatReservation.ParticipantUserId);
                }
            }

            TotalAvailableSeats = seatsCount;

            return Result.Successful;
        }

        public bool AreSeatsAvailable => _seatReservations.Count < TotalAvailableSeats;
        public int AvailableSeats => TotalAvailableSeats - _seatReservations.Count;

        public Result ReserveSeat(Guid userId, DateTimeOffset when)
        {
            if (!AreSeatsAvailable)
            {
                return Result.Failure("All seats are already taken");
            }

            var seatReservation = new SeatReservation(userId, when);
            _seatReservations.Add(seatReservation);

            return Result.Successful;
        }

        public Result CancelSeatReservation(Guid userId)
        {
            _seatReservations.RemoveAll(x => x.ParticipantUserId == userId);

            return Result.Successful;
        }

        internal Meetup() { }

        private List<SeatReservation> _seatReservations = new List<SeatReservation>();
    }

    public class SeatReservation
    {
        public int Id { get; private set; }
        public Guid ParticipantUserId { get; private set; }
        public DateTimeOffset ReservationDate { get; private set; }

        internal SeatReservation(Guid participantUserId, DateTimeOffset reservationDate)
        {
            ParticipantUserId = participantUserId;
            ReservationDate = reservationDate;
        }

        internal SeatReservation() { }
    }
}
