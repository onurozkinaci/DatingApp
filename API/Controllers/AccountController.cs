﻿using System.Security.Cryptography;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using API.Interfaces;
using AutoMapper;

namespace API.Controllers;

public class AccountController:BaseApiController
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
    {
        _context = context;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpPost("register")] //POST: ...../api/account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
      if(await UserExists(registerDto.Username)) return BadRequest("Username is taken!");

       var user = _mapper.Map<AppUser>(registerDto);

      //*=>instance'in isi bitince otomatik olarak dispose edilmesi icin 'using' kullanildi.
       using var hmac = new HMACSHA512(); //=>randomly generated key will be taken as PasswordSalt property at below.
       user.UserName = registerDto.Username.ToLower();
       user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
       user.PasswordSalt = hmac.Key; //=>comes with HMACSHA512() call directly.
       _context.Users.Add(user);
       await _context.SaveChangesAsync();

       return new UserDto
       {
         Username = user.UserName,
         Token = _tokenService.CreateToken(user),
         KnownAs = user.KnownAs
       };
    }

    [HttpPost("login")] //POST: ...../api/account/login
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
       var user = await _context.Users
               .Include(p => p.Photos) //to get the photos of the user with eager loading
               .SingleOrDefaultAsync(x=>x.UserName == loginDto.Username.ToLower());
       
       if(user == null) return Unauthorized("invalid username!");
       
       //----------------------
       //*=>To compare the entered password with the saved hashed password in the db by using PasswordSalt (key info);
       using var hmac = new HMACSHA512(user.PasswordSalt);
       var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
       //=>since hashed password is in byte array format;
       for (int i = 0; i < computedHash.Length; i++)
       {
          if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password!");
       }
       //----------------------
       return new UserDto
       {
         Username = user.UserName,
         Token = _tokenService.CreateToken(user),
         PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain)?.Url, //to get the main photo of the logged in user
         KnownAs = user.KnownAs
       };
    }

    //=>Kullanicinin onceden kayit olup olmadiginin kontrolu icin Db'ye gideceginden async tanimlanir;
    public async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(x=>x.UserName == username.ToLower());
    }
}
