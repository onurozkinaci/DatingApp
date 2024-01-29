using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class UsersController : BaseApiController
{
    private readonly DataContext _context;

    public UsersController(DataContext context)
    {
        _context = context;
    }

    //=>------ENDPOINTS------;
    [HttpGet] //e.g. 'https://localhost:5001/api/users'
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return users; /*=>donus tipi belirttigimizden AppUser tipine ait HttpResponse donebiliriz. Belirtmeseydik 200 veya `Ok(users)` ile
        donersek daha acik bir success message donerdi (200 yerine).*/

    }

    [HttpGet("{id}")] //api/users/id(2,3,etc.) -> e.g. 'https://localhost:5001/api/users/1'
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        return await _context.Users.FindAsync(id); //=>Finds an entity with the given primary key values.
    }
}
