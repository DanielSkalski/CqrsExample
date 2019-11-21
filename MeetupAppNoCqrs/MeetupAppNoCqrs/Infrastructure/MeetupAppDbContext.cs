using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;

namespace MeetupAppNoCqrs.Infrastructure
{
    public class MeetupAppDbContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public DbSet<Domain.Meetup> Meetups { get; set; }
        public DbSet<Domain.UserProfile> UserProfiles { get; set; }

        public MeetupAppDbContext(
            DbContextOptions<MeetupAppDbContext> options,
            ILoggerFactory loggerFactory)
            : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new MeetupEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserProfileEntityTypeConfiguration());
        }

        class MeetupEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Meetup>
        {
            public void Configure(EntityTypeBuilder<Domain.Meetup> builder)
            {
                builder.HasKey(x => x.Id);
            }
        }

        class UserProfileEntityTypeConfiguration : IEntityTypeConfiguration<Domain.UserProfile>
        {
            public void Configure(EntityTypeBuilder<Domain.UserProfile> builder)
            {
                builder.HasKey(x => x.Id);
            }
        }
    }
}
