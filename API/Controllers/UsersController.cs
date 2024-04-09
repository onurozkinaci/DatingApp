using System.Security.Claims;
using API.DTOS;
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

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        //=>Token uzerinden name identifier olarak atanan username'e erisecegiz(profilini duzenleyen user icin);
       var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
       var user = await _userRepository.GetUserByUsernameAsync(username);
       if(user == null) return NotFound();

       _mapper.Map(memberUpdateDto,user); //=>updates the properties of user by overwritting them automatically from the datas that sent with memberUpdateDto via AutoMapper.
       if(await _userRepository.SaveAllAsync()) return NoContent(); //=>everything is okay but nothing sent back(changes saved to db successfully)

       return BadRequest("Failed to update user!");
    }
}
