using BrainstormSessions.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace BrainstormSessions.Infrastructure
{
    public class AppDbContext : DbContext
    {

        private readonly Serilog.ILogger _logger;

        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions, Serilog.ILogger logger) :
            base(dbContextOptions)
        {
            _logger = logger;
            _logger.Information("AppDbContext initialized!");
        }

        public DbSet<BrainstormSession> BrainstormSessions { get; set; }
    }
}
