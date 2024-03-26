using API.Entities;

namespace API;

public interface IUserRepository
{
  void Update(AppUser user);
  Task<bool> SaveAllAsync();
  Task<IEnumerable<AppUser>> GetUsersAsync();
  Task<AppUser> GetUserByIdAsync(int id);
  Task<AppUser> GetUserByUsernameAsync(string username);

  //----**To provide optimization for the queries with AutoMapper return types are given as MemberDto other than AppUser and these methods will be filled with the operations(LINQ queries) for this Dto at the implementation class(UserRepository);
  Task<IEnumerable<MemberDto>> GetMembersAsync();
  Task<MemberDto>GetMemberAsync(string username);
  //-------------------------------------------------
}
