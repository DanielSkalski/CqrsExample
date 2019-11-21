using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MeetupAppNoCqrs.Infrastructure;
using MeetupAppNoCqrs.Meetup;
using MeetupAppNoCqrs.UserProfile;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace MeetupAppNoCqrs
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = "Data source=MeetupsApp.db";

            services.AddEntityFrameworkSqlite();
            services.AddDbContext<MeetupAppDbContext>(options => options.UseSqlite(connectionString));

            services.AddScoped<MeetupApplicationService>();
            services.AddScoped<MeetupRepository>();

            services.AddScoped<UserProfileApplicationService>();
            services.AddScoped<UserProfileRepository>();

            services.AddMvc(options => options.EnableEndpointRouting = false)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });
        }

        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            MeetupAppDbContext dbContext)
        {
            EnsureDatabaseIsCreated(dbContext);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
        }

        private void EnsureDatabaseIsCreated(MeetupAppDbContext dbContext)
        {
            dbContext.Database.Migrate();
            DatabaseSeed.Seed(dbContext);
        }
    }
}
