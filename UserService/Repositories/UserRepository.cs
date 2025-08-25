using UserService.Data;
using UserService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
    {
        _context = context;
    }
    public async Task<User> GetUserByMailAsync(string mail)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Mail == mail);
        return user;
    }

    public async Task<User> LoginUser(string mail, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Mail == mail);
        if (user != null)
        {
            return user;
        }
        return null;
    }

    public async Task<User> RegisterUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteUserById(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return false;
        }
        user.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<User> GetUserById(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        return user;
    }

}
