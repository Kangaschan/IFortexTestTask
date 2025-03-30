using Lab5TestTask.Data;
using Lab5TestTask.Enums;
using Lab5TestTask.Models;
using Lab5TestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab5TestTask.Services.Implementations;

/// <summary>
/// SessionService implementation.
/// Implement methods here.
/// </summary>
public class SessionService : ISessionService
{
    private readonly ApplicationDbContext _dbContext;

    public SessionService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
//method of SessionService so it returns the first(earliest) desktop Session
    public async Task<Session> GetSessionAsync()
    {
        return await _dbContext.Sessions
            .Where(s => s.DeviceType == DeviceType.Desktop)
            .OrderBy(s => s.StartedAtUTC)
            .FirstOrDefaultAsync();
        
    }
//method of SessionService so it returns only Sessions from Active users that were ended before 2025
    public async Task<List<Session>> GetSessionsAsync()
    {
        return await _dbContext.Sessions
            .Include(s => s.User)
            .Where(s => s.User.Status == UserStatus.Active && s.EndedAtUTC.Year < 2025)
            .Select(s => new Session 
            {
                Id = s.Id,
                StartedAtUTC = s.StartedAtUTC,
                EndedAtUTC = s.EndedAtUTC,
                DeviceType = s.DeviceType,
                UserId = s.UserId,
                User = new User 
                {
                    Id = s.User.Id,
                    Email = s.User.Email,
                    Status = s.User.Status
                }
            })
            .ToListAsync();
    }
}
