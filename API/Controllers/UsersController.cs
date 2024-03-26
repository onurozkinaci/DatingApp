using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        var users = await _userRepository.GetMembersAsync();
        return Ok(users);
    }

    [HttpGet("{username}")] //api/users/username -> e.g. 'https://localhost:5001/api/users/christie'
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        //=>Since the mapping from AppUser to MemberDto is detected directly with the "ProjectTo" in GetMemberAsync(), the _mapper.Map doesnt needed anymore here;
        return await _userRepository.GetMemberAsync(username); 
    }
}
