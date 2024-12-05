using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrainstormSessions.Core.Interfaces;
using BrainstormSessions.Core.Model;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BrainstormSessions.Infrastructure
{
    public class EFStormSessionRepository : IBrainstormSessionRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly Serilog.ILogger _logger;

        public EFStormSessionRepository(AppDbContext dbContext, Serilog.ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<BrainstormSession> GetByIdAsync(int id)
        {
            _logger.Information("Fetching BrainstormSession with ID: {Id}", id);

            try
            {
                var session = await _dbContext.BrainstormSessions
                    .Include(s => s.Ideas)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (session == null)
                {
                    _logger.Warning("No BrainstormSession found with ID: {Id}", id);
                }
                else
                {
                    _logger.Information("Successfully fetched BrainstormSession with ID: {Id}", id);
                }

                return session;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, "Error fetching BrainstormSession with ID: {Id}", id);
                throw;
            }
        }

        public async Task<List<BrainstormSession>> ListAsync()
        {
            _logger.Information("Fetching all BrainstormSessions.");

            try
            {
                var sessions = await _dbContext.BrainstormSessions
                    .Include(s => s.Ideas)
                    .OrderByDescending(s => s.DateCreated)
                    .ToListAsync();

                _logger.Information("Successfully fetched {Count} BrainstormSessions.", sessions.Count);
                return sessions;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, "Error fetching BrainstormSessions.");
                throw;
            }
        }

        public async Task AddAsync(BrainstormSession session)
        {
            _logger.Information("Adding a new BrainstormSession: {SessionName}", session.Name);

            try
            {
                _dbContext.BrainstormSessions.Add(session);
                await _dbContext.SaveChangesAsync();

                _logger.Information("Successfully added BrainstormSession: {SessionName}", session.Name);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, "Error adding BrainstormSession: {SessionName}", session.Name);
                throw;
            }
        }

        public async Task UpdateAsync(BrainstormSession session)
        {
            _logger.Information("Updating BrainstormSession with ID: {Id}", session.Id);

            try
            {
                _dbContext.Entry(session).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                _logger.Information("Successfully updated BrainstormSession with ID: {Id}", session.Id);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, "Error updating BrainstormSession with ID: {Id}", session.Id);
                throw;
            }
        }
    }
}
