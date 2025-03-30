using Lab5TestTask.Data;
using Lab5TestTask.Enums;
using Lab5TestTask.Models;
using Lab5TestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab5TestTask.Services.Implementations;

/// <summary>
/// UserService implementation.
/// Implement methods here.
/// </summary>
public class UserService : IUserService
{
    private readonly ApplicationDbContext _dbContext;

    public UserService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    //method of UserService so it returns a User with the biggest amount of sessions
    public async Task<User> GetUserAsync()
    {
        return await _dbContext.Users
            .Select(u => new User  
            {
                Id = u.Id,
                Email = u.Email,
                Status = u.Status,
            
                Sessions = u.Sessions.Select(s => new Session
                {
                    Id = s.Id,
                    StartedAtUTC = s.StartedAtUTC,
                    EndedAtUTC = s.EndedAtUTC,
                    DeviceType = s.DeviceType,
                    UserId = s.UserId,
                    User = null 
                }).ToList()
            })
            .OrderByDescending(u => u.Sessions.Count)
            .FirstOrDefaultAsync(); 
    }
    //method of UserService so it returns Users that has at least 1 Mobile session


    public async Task<List<User>> GetUsersAsync()
    {
        return await _dbContext.Users
            .Where(u => u.Sessions.Any(s => s.DeviceType == DeviceType.Mobile))
            .ToListAsync();
    }
}
