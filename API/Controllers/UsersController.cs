using API.Data;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }   

    //=>------ENDPOINTS------;
    [HttpGet] //e.g. 'https://localhost:5001/api/users'
    //**=>NOTE::MemberDto is kind of a reflection of AppUser and the props between them will be compared with AutoMapper;
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await _userRepository.GetUsersAsync();
        var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
        return Ok(usersToReturn);
    }

    [HttpGet("{username}")] //api/users/username -> e.g. 'https://localhost:5001/api/users/christie'
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        var user =  await _userRepository.GetUserByUsernameAsync(username);
        return Ok(_mapper.Map<MemberDto>(user));
    }

    /*[HttpGet("{id}")] //api/users/id(2,3,etc.) -> e.g. 'https://localhost:5001/api/users/1'
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        return await _userRepository.GetUserByIdAsync(id); //=>Finds an entity with the given primary key value.
    }*/

    
}
