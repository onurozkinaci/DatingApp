using System.Security.Claims;
using API.DTOS;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;

    public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _photoService = photoService;
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
        //=>"User.GetUsername()" extension metodu ile token uzerinden name identifier olarak atanan username'e erisecegiz(profilini duzenleyen user icin);
       var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
       if(user == null) return NotFound();

       _mapper.Map(memberUpdateDto,user); //=>updates the properties of user by overwritting them automatically from the datas that sent with memberUpdateDto via AutoMapper.
       if(await _userRepository.SaveAllAsync()) return NoContent(); //=>everything is okay but nothing sent back(changes saved to db successfully)

       return BadRequest("Failed to update user!");
    }
    
    [HttpPost("add-photo")]
     public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
     {
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
        if(user == null) return NotFound();
        
        //-----------------
        //=>Adding new photo to user's profile;
        var result = await _photoService.AddPhotoAsync(file);
        if(result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        if(user.Photos.Count == 0) photo.IsMain = true;
        user.Photos.Add(photo); //=>user and this operation is tracked by EF in memory since its obtained from the repository above.
        
        if(await _userRepository.SaveAllAsync())
        {
            //=>CreatedAtAction is used to return response 201 which contains the 'Location' header of GetUser endpoint(e.g. https://localhost:5001/api/Users/christie) to show where to find the uploaded photo and the last parameter is used to show the PhotoDto object in the response of this('AddPhoto') endpoint.
           return CreatedAtAction(nameof(GetUser), new {username = user.UserName}, _mapper.Map<PhotoDto>(photo));
        }

        return BadRequest("Problem occured while adding the photo!"); //if photo saving to db fails
        //-----------------

     }

     [HttpPut("set-main-photo/{photoId}")]
     public async Task <ActionResult> SetMainPhoto(int photoId)
     {
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
        if(user == null) return NotFound();
        
        //to get the photo which is gonna be updated;
        var photo = user.Photos.FirstOrDefault(x=>x .Id == photoId);
        if(photo == null) return NotFound();

        //doesnt need to be set as main photo of the user if its already a main photo;
        if(photo.IsMain) return BadRequest("This is already your main photo!");

        var currentMain = user.Photos.FirstOrDefault(x=>x.IsMain);
        if(currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;

        //=>The tracked changes by EF will be signed to db with SaveChanges() below;
        if(await _userRepository.SaveAllAsync()) return NoContent(); //since its updating the resource, not creating it
        
        return BadRequest("Problem occured while setting the main photo!");
     }

     [HttpDelete("delete-photo/{photoId}")]
     public async Task<ActionResult> DeletePhoto(int photoId)
     {
         var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
         if(user == null) return NotFound();
         
         var photo = user.Photos.FirstOrDefault(x=>x.Id == photoId);
         if(photo == null) return NotFound();

         if(photo.IsMain) return BadRequest("You cannot delete your main photo!");
         if(photo.PublicId != null)
         { 
             var result = await _photoService.DeletePhotoAsync(photo.PublicId);
             if(result.Error != null) return BadRequest(result.Error.Message);
         }

         user.Photos.Remove(photo);
         //=>Changes for the tracked user entity by EF will be reflected on Db;
         if(await _userRepository.SaveAllAsync()) return Ok();
        
         return BadRequest("Problem deleting photo!");
     }
}
