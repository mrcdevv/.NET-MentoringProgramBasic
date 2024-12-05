using System;
using System.Linq;
using System.Threading.Tasks;
using BrainstormSessions.Core.Interfaces;
using BrainstormSessions.Core.Model;
using BrainstormSessions.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BrainstormSessions
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(
                optionsBuilder => optionsBuilder.UseInMemoryDatabase("InMemoryDb"));

            services.AddSingleton(Log.Logger);

            services.AddControllersWithViews();

            services.AddScoped<IBrainstormSessionRepository, EFStormSessionRepository>();
        }

        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            IServiceProvider serviceProvider)
        {

            var logger = serviceProvider.GetRequiredService<Serilog.ILogger>();

            if (env.IsDevelopment())
            {
                logger.Information("Development enviroment...");

                var repository = serviceProvider.GetRequiredService<IBrainstormSessionRepository>();

                try
                {
                    InitializeDatabaseAsync(repository, logger).Wait();
                    logger.Information("Database initialized successfully...");
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Error initializing the database!");
                }

            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            logger.Information("Application configured successfully!");
        }

        public async Task InitializeDatabaseAsync(IBrainstormSessionRepository repo, Serilog.ILogger logger)
        {
            logger.Debug("Initializing database...");
            var sessionList = await repo.ListAsync();
            if (!sessionList.Any())
            {
                await repo.AddAsync(GetTestSession());
                logger.Information("Test session added to the database!");
            }
        }

        public static BrainstormSession GetTestSession()
        {
            var session = new BrainstormSession()
            {
                Name = "Test Session 1",
                DateCreated = new DateTime(2016, 8, 1)
            };
            var idea = new Idea()
            {
                DateCreated = new DateTime(2016, 8, 1),
                Description = "Totally awesome idea",
                Name = "Awesome idea"
            };
            session.AddIdea(idea);
            return session;
        }
    }
}
