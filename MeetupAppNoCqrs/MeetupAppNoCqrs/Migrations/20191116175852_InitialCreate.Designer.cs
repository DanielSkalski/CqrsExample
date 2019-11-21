﻿// <auto-generated />
using System;
using MeetupAppNoCqrs.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MeetupAppNoCqrs.Migrations
{
    [DbContext(typeof(MeetupAppDbContext))]
    [Migration("20191116175852_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0");

            modelBuilder.Entity("MeetupAppNoCqrs.Domain.FriendLink", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FriendUserId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("UserProfileId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserProfileId");

                    b.ToTable("FriendLink");
                });

            modelBuilder.Entity("MeetupAppNoCqrs.Domain.Meetup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("HostUserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalAvailableSeats")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Meetups");
                });

            modelBuilder.Entity("MeetupAppNoCqrs.Domain.SeatReservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MeetupId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ParticipantUserId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("ReservationDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MeetupId");

                    b.ToTable("SeatReservation");
                });

            modelBuilder.Entity("MeetupAppNoCqrs.Domain.UserProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DisplayName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("MeetupAppNoCqrs.Domain.FriendLink", b =>
                {
                    b.HasOne("MeetupAppNoCqrs.Domain.UserProfile", null)
                        .WithMany("Friends")
                        .HasForeignKey("UserProfileId");
                });

            modelBuilder.Entity("MeetupAppNoCqrs.Domain.SeatReservation", b =>
                {
                    b.HasOne("MeetupAppNoCqrs.Domain.Meetup", null)
                        .WithMany("SeatReservations")
                        .HasForeignKey("MeetupId");
                });
#pragma warning restore 612, 618
        }
    }
}
