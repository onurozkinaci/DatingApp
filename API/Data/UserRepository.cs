using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<AppUser> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
        return await _context.Users 
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x=>x.UserName.ToLower() == username.ToLower());
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await _context.Users
              .Include(p => p.Photos) //to list also the related entity which is Photos with the users.
              .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
        //**=>To tell EF Tracker that an entity has changed. It is also detected automatically when you change an entity inside of a method if you dont define it explicitly;
        _context.Entry(user).State = EntityState.Modified; 

        /*var updatedUser = _context.Users.FirstOrDefault(x=>x.UserName.ToLower() == user.UserName.ToLower());
        if(updatedUser != null)
        {
            updatedUser.Interests = user.Interests;
            updatedUser.Introduction = user.Introduction;
            updatedUser.DateOfBirth = user.DateOfBirth;
            updatedUser.KnownAs = user.KnownAs;
            updatedUser.LookingFor = user.LookingFor;
            updatedUser.City = user.City;
            updatedUser.Country = user.Country;
            updatedUser.Photos = user.Photos;
            _context.Users.ex
        }*/
    }
}
