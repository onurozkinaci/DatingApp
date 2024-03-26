using API.Data;
using API.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UserRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    //-----AutoMapper optimization;
    public async Task<MemberDto> GetMemberAsync(string username)
    {
        return await _context.Users
             .Where(x=>x.UserName == username)
             .ProjectTo<MemberDto>(_mapper.ConfigurationProvider) //Finds our MappingProfile class from the service that we added to our 'applicationserviceextensions' and mapping from AppUser to MemberDto will be detected.
             .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<MemberDto>> GetMembersAsync()
    {
       return await _context.Users
              .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
              .ToListAsync();
    }
    //-------------------------------

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
    }
}
