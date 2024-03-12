using API.Extensions;

namespace API.Entities;

public class AppUser
{
  public int Id { get; set; }
  public string UserName { get; set; }
  public byte[] PasswordHash { get; set; }
  public byte[] PasswordSalt { get; set; }
  public DateOnly DateOfBirth { get; set; } //DateOnly is used to track only the date of something.
  public string KnownAs { get; set; } //unique as username
  public DateTime Created { get; set; } = DateTime.UtcNow; //creation time of the new user
  public DateTime LastActive { get; set; } = DateTime.UtcNow; //last time the user is active in the app
  public string Gender { get; set; }
  public string Introduction { get; set; }
  public string LookingFor { get; set; }
  public string Interests { get; set; }
  public string City { get; set; }
  public string Country { get; set; }

  //=>one-to-many relationship;
  public List<Photo> Photos { get; set; } = new(); //new List<Photo>()

  public int GetAge()
  {
    return DateOfBirth.CalculateAge();
  }
}